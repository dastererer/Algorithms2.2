namespace Algorithms
{
    class BFS2D
    {
        public static int[,] RunAlgorithm(char[,] map, (int x, int y) startPos)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);
            int[,] distances = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    {
                        distances[i, j] = -1;
                    }
            }

            Queue<(int x, int y)> queue = new Queue<(int x, int y)>();

            distances[startPos.x, startPos.y] = 0;
            queue.Enqueue(startPos);
            while (queue.Count > 0)
            {
                var (currX, currY) = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int nextX = currX + Tools.Dx[i];
                    int nextY = currY + Tools.Dy[i];

                    if (nextX >= 0 && nextX < rows && nextY >= 0 && nextY < cols)
                    {
                        if (map[nextX, nextY] != '#' && distances[nextX, nextY] == -1)
                        {
                            distances[nextX, nextY] = distances[currX, currY] + 1;
                            queue.Enqueue((nextX, nextY));
                        }
                    }
                }
            }
            return distances;
        }
    }
}

