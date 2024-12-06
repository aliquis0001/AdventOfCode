namespace Advent2022{
    class Day1{
        public static (int,int) dayResult(){
            using StreamReader infile = File.OpenText("input01");
            {
                List<int> e = new List<int>();
                String? s;
                int amt = 0;
                while ((s=infile.ReadLine())!= null){
                    if(s!=""){
                        amt+=(int.Parse(s));
                    }
                    else{
                        e.Add(amt);
                        amt=0;
                    }
                }
                e = e.OrderByDescending(x=>x).ToList();
                return (e[0],e.GetRange(0,3).Sum());
            }
        }
    }
}