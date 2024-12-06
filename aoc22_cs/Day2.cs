namespace Advent2022{
    class Day2{
        public static (int, int) dayResult(){
            List<String> opponent = new List<String>{"","A","B","C"};
            List<String> you = new List<String>{"","X","Y","Z"};
            List<int> p1outcome = new List<int>{3, 0, 6};
            List<int> p2outcome = new List<int> {0,0,3,6};
            
            using StreamReader infile = File.OpenText("input02");
            {
                String? s;
                int p1 = 0;
                int p2 = 0;
                while ((s=infile.ReadLine())!= null){
                    var y = you.IndexOf(s.Substring(s.Length-1));
                    var o = opponent.IndexOf(s.Substring(0,1));
                    p1+= y+p1outcome[(o-y+3)%3];
                    p2 += p2outcome[y];
                    switch (y)
                    {
                        case 1:
                            p2 += o-1==0?3:o-1;
                            break;
                        case 2:
                            p2 += o;
                            break;
                        case 3:
                            p2 += o+1==4?1:o+1;
                            break;
                    }
                }
                return(p1, p2);
            }
        }
    }
}