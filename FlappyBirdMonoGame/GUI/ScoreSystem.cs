namespace FlappyBirdMonoGame.GUI
{
    public class ScoreSystem
    {
        public int HighScores { get; private set; }
        public int Scores { get; private set; }

        public ScoreSystem()
        {
        }

        public void ResetScore()
        {
            Scores = 0;
        }

        public void AddScore()
        {
            Scores++;
            if (Scores > HighScores)
            {
                HighScores = Scores;
            }
        }

    }
}
