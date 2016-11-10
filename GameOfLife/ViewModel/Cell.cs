namespace GameOfLife.ViewModel
{
    public class Cell
    {
        private int age;
        private bool isAlive;

        public Cell(int age, bool isAlive)
        {
            this.age = age;
            this.isAlive = isAlive;
        }

        public int Age
        {
            get { return age; }
            set { age = value; }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
    }
}