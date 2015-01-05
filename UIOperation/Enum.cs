namespace HP.LR.Test.UIOperation
{
    public class PatternEnum
    {
        public enum DockPosition
        {
            Top = 0,
            Left = 1,
            Bottom = 2,
            Right = 3,
            Fill = 4,
            None = 5,
        }

        public enum WindowVisualState
        {
            Normal = 0,
            Maximized = 1,
            Minimized = 2,
        }

        public enum ScrollAxis
        {
            Horizontal = 0,
            Vertical = 1, 
            Both = 2,
        }

        public enum ScrollAmount
        {
            LargeDecrement = 0,
            SmallDecrement = 1,
            NoAmount = 2,
            LargeIncrement = 3,
            SmallIncrement = 4,
        }

        public enum WindowInteractionState
        {
            Running = 0,
            Closing = 1,
            ReadyForUserInteraction = 2,
            BlockedByModalWindow = 3,
            NotResponding = 4,
        }

        public enum Axis
        {
            Row = 0,
            Column = 1,
        }

        public enum RowOrColumnMajor
        {
            RowMajor = 0,
            ColumnMajor = 1,
            Indeterminate = 2,
        }

        public enum RangeValueInformation
        {
            Value = 0,
            LargeChange = 1,
            SmallChange = 2,
            Max = 3,
            Min = 4,
        }

        public enum ToggleState
        {
            ToggleOn = 0,
            ToggleOff = 1,
        }
    }
}
