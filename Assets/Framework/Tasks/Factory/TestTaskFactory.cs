using UnityEngine;
using System.Collections;

public class TestTaskFactory : TaskFactory {

#region Implement Virtual Functions
	public override Task CreateTask()
	{
		Task mainTask = new ParallelTasks(this);

		for(int i = 0; i < 10; i ++)
		{
			(mainTask as ParallelTasks).AddTask(new Task(this));
		}

		for(int i = 0; i < 3; i ++)
		{
			(mainTask as ParallelTasks).AddTask(new LinkListTask(this));
		}

		SeriesTasks sTask1 = new SeriesTasks(this);
		SeriesTasks sTask2 = new SeriesTasks(this);

		for(int i = 0; i < 2; i ++)
		{
			sTask1.AddTask(new Task(this));
		}

		ParallelTasks pTask1 = new ParallelTasks(this);

		for(int i = 0; i < 5; i ++)
		{
			pTask1.AddTask(new LinkListTask(this));
		}

		for(int i = 0; i < 7; i ++)
		{
			sTask2.AddTask(new Task(this));
		}

		sTask1.AddTask(pTask1);
		sTask1.AddTask(sTask2);

		(mainTask as ParallelTasks).AddTask(sTask1);

		return mainTask;
	}
#endregion
}
