using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PsMachineTools.Tools;

namespace PsMachineWork
{
    /// <summary>创建的TASK，一个TASK可以执行多个不同地方的任务
    /// </summary>
    internal class WorkTaskRun : SynCollection<WorkTaskRunFunc>, iWorkTaskRun,
        INotifyCollectionChanged
    {

        /// <summary>任务的KEY
        /// </summary>
        public string TaskID
        {
            get { return _TaskID; }
            internal set
            {
                var myChanged = _TaskID != value;
                if (myChanged)
                {
                    _TaskID = value;
                    OnPropertyChanged(nameof(TaskID));
                }
            }
        }
        string _TaskID;


        /// <summary>任务注释
        /// </summary>
        public string TaskDesc
        {
            get { return _TaskDesc; }
            internal set
            {
                var myChanged = _TaskDesc != value;
                if (myChanged)
                {
                    _TaskDesc = value;
                    OnPropertyChanged(nameof(TaskDesc));
                }
            }
        }
        string _TaskDesc;


        /// <summary>执行完一个循环之后,在进行下一个循环之前,停止刷新的时间
        /// 用于防止一些点位或者事务特别多的,高频地循环把CPU吃满
        /// </summary>
        public int TimeDelay
        {
            get { return _TimeDelay; }
            internal set
            {
                var myChanged = _TimeDelay != value;
                if (myChanged)
                {
                    _TimeDelay = value;
                    OnPropertyChanged(nameof(TimeDelay));
                }
            }
        }
        int _TimeDelay = 0;

        /// <summary>添加新的执行函数，如果间隔时间比原来小，则会取最小值
        /// </summary>
        public WorkTaskRunFunc Add(string myTaskDesc, Func<CancellationToken, bool> myFunc, int myTimeDelay = 1000)
        {
            if (myTimeDelay < 0)
                myTimeDelay = 0;

            if (this.TimeDelay > myTimeDelay)
            {
                this.TimeDelay = myTimeDelay;
            }

            var myWorkTaskRunFunc = new WorkTaskRunFunc()
            {
                TaskDesc = myTaskDesc,
                Func = myFunc,
            };
            this.Add(myWorkTaskRunFunc);
            return myWorkTaskRunFunc;
        }


        public async void Run_TaskWhile(CancellationToken myCancellationToken)
        {
            bool myContinue = true;
            while (myContinue)
            {
                if (myCancellationToken.IsCancellationRequested)
                    break;
                myContinue = Run_TaskContinue(myCancellationToken);

                if (!myContinue)
                {
                    this.CloseEvent?.Invoke(this, new EventArgs());
                }

                if (myCancellationToken.IsCancellationRequested)
                    break;

                await Task.Delay(TimeDelay);
            }
        }

        /// <summary>返回 false 时表示 Task终止
        /// </summary>
        /// <returns></returns>
        private bool Run_TaskContinue(CancellationToken myCancellationToken)
        {
            List<WorkTaskRunFunc> myFuncList = this.CloneList();
            if (myFuncList.Count == 0)
            {
                return false;
            }

            foreach (var myFuncOneWork in myFuncList)
            {
                if (myCancellationToken.IsCancellationRequested)
                    return false;

                try
                {
                    var myContinue = myFuncOneWork.Func(myCancellationToken);
                    if (!myContinue)
                    {
                        this.Remove(myFuncOneWork);
                    }
                }
                catch (Exception)
                {
                }
            }
            if (myCancellationToken.IsCancellationRequested)
                return false;

            return true;
        }

        IEnumerator<iWorkTaskRunFunc> IEnumerable<iWorkTaskRunFunc>.GetEnumerator()
        {
            return this.GetEnumerator();
        }


        /// <summary>Task关闭事件
        /// </summary>
        public event EventHandler<EventArgs> CloseEvent;


    }


}
