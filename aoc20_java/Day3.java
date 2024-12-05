import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class Day3 {
    public static void main(String[] args) {
        Scanner inFile = null;
        try {   //Create Scanner from file
            inFile = new Scanner(new File("input3.txt"));
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
        long start = System.currentTimeMillis();
        String[][] map = new String[323][31];
        int count = 0;
        inFile.useDelimiter("");
        for (int r = 0; r < 323; r++) {
            for (int c = 0; c < 31; c++) {
                map[r][c] = inFile.next();
                count++;
            }
            if (inFile.hasNext())
                inFile.next();  //get rid of the newline string at the end of each line.
        }
        long oneOne = numTrees(1, 1, map);
        long threeOne = numTrees(3, 1, map);
        long fiveOne = numTrees(5, 1, map);
        long sevenOne = numTrees(7, 1, map);
        long oneTwo = numTrees(1, 2, map);
        System.out.println(oneOne + " " + threeOne + " " + fiveOne + " " + sevenOne + " " + oneTwo + " " + (oneOne * threeOne * fiveOne * sevenOne * oneTwo));
        System.out.println(System.currentTimeMillis() - start);
    }

    static long numTrees(int right, int down, String[][] map) {
        int numTrees = 0;
        int r = 0;
        int c = 0;
        while (r < 323) {
            if (map[r][c].equals("#")) {
                numTrees++;
            }
            r += down;
            c += right;
            if (c >= 31)
                c -= 31;
        }
        return numTrees;
    }
}
