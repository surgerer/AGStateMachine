﻿namespace AGStateMachine.Tests
{
    public  class TestSM1
    {
        public enum StateSM1
        {
            First,
            Second
        }

        public enum EventSM1
        {
            FirstEvent
        }

     
        public class TypedEvent
        {
            public int Counter { get; set; }
        }

        public class TestStateInstance : IInstance<StateSM1>
        {
            public StateSM1 CurrentState { get; set; }
            public int Counter { get; set; }
            public virtual void Invoke(int seq){}
        }

        public class TestStateMachine1 : AGStateMachine<StateSM1, EventSM1, TestStateInstance>
        {
        }

        public static TestStateMachine1 Create()
        {
            return  new TestStateMachine1();
        }
    }
}