using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PsMachineTools.Tools;

namespace PsMachineWork
{
    internal class WorkTaskRunCollection : SynCollection<WorkTaskRun>, iWorkTaskManager // iWorkTaskRunCollection<iWorkTaskRun>
    {

        public WorkTaskRunCollection()
        {
            m_CancellationTokenSource = new CancellationTokenSource();
        }


        public CancellationTokenSource m_CancellationTokenSource;


        /// <summary>检查和创建新的Task线程，如果间隔时间比原来小，则取最小值
        /// </summary>
        /// <param name="myTaskID">一般不能为null</param>
        /// <param name="myRunFunc"></param>
        /// <param name="myRunException"></param>
        /// <returns></returns>
        public iWorkTaskRun CreateTask(string myTaskID, string myDesc, Func<CancellationToken, bool> myRunFunc, int myTimeDelay = 1000)
        {
            if (myTimeDelay < 0)
                myTimeDelay = 0;

            if (myRunFunc == null)
            {
                throw new Exception("添加线程任务异常，myRunFunc 参数不能为空");
            }

            if (string.IsNullOrEmpty(myTaskID))
            {
                myTaskID = Guid.NewGuid().ToString();
            }

            WorkTaskRun myRunTask;
            var list = this.CloneList();
            myRunTask = list.FirstOrDefault(p => p.TaskID == myTaskID);
            if (myRunTask == null)
            {
                myRunTask = new WorkTaskRun()
                {
                    TaskID = myTaskID,
                    TaskDesc = myDesc,
                    TimeDelay = myTimeDelay,
                };
                myRunTask.CloseEvent += Item_CloseEvent;
                myRunTask.Add(myDesc, myRunFunc, myTimeDelay);
                Task.Run(() => myRunTask.Run_TaskWhile(m_CancellationTokenSource.Token));
                this.Add(myRunTask);
            }
            else
            {
                myRunTask.Add(myDesc, myRunFunc, myTimeDelay);
            }

            return myRunTask;
        }


        IEnumerator<iWorkTaskRun> IEnumerable<iWorkTaskRun>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Close()
        {
            if (m_CancellationTokenSource != null)
            {
                m_CancellationTokenSource.Cancel();
            }
            m_CancellationTokenSource = null;
        }


        private void Item_CloseEvent(object sender, EventArgs e)
        {
            var mySender = sender as WorkTaskRun;
            if (mySender == null)
                return;

            this.Remove(mySender);
            mySender.CloseEvent -= Item_CloseEvent;
        }


    }
}
