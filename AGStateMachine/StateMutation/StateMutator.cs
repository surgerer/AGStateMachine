﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AGStateMachine;

namespace DefaultNamespace
{
    public class StateMutator<TInstance, TState>
        where TInstance : IInstance<TState>
        where TState : Enum
    {
        private readonly ActionBlock<Func<Task>> _block;
        private readonly CancellationTokenSource _cts;

        public StateMutator()
        {
            _cts = new CancellationTokenSource();
            var option = new ExecutionDataflowBlockOptions {CancellationToken = _cts.Token};
            _block = new ActionBlock<Func<Task>>(f => f(), option);
            _block.Completion.ContinueWith((t) => { Console.WriteLine("Block stopped"); });
        }

        public Task SendAsync(TInstance instance, TState currentState, Func<TInstance, Task> func)
        {
            var tcs = new TaskCompletionSource<bool>();
            _block.SendAsync(async () =>
            {
                try
                {
                    if (!Equals(instance.CurrentState, currentState))
                        return;
                    await func(instance);
                    tcs.SetResult(true);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });

            return tcs.Task;
        }


        ~StateMutator()
        {
            _cts.Cancel();
        }
    }
}