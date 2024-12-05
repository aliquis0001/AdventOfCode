import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class Day2 {
    public static void main(String[] args) {
        Scanner inFile = null;
        try {   //Create Scanner from file
            inFile = new Scanner(new File("input2.txt"));
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
        long start = System.currentTimeMillis();
        int p1matches = 0;
        int p2matches = 0;
        while (inFile.hasNextLine()) {
            String[] limits = inFile.next().split("-"); //apparently nextInt can't grab an int if there's a character straight after it
            int[] ints = new int[2];
            ints[0] = Integer.parseInt(limits[0]);
            ints[1] = Integer.parseInt(limits[1]);
            char c = inFile.next().charAt(0);
            String pass = inFile.next();
            String regex = regexBuilder(ints[0], ints[1], c);
            if (pass.matches(regex))
                p1matches++;
            if (pass.charAt(ints[0] - 1) == c ^ pass.charAt(ints[1] - 1) == c)
                p2matches++;
        }
        System.out.println(p1matches);
        System.out.println(p2matches);
        System.out.println(System.currentTimeMillis() - start);
    }

    static String regexBuilder(int n, int x, char y) {
        return "^(?:[^" + y + "]*[" + y + "]){" + n + "," + x + "}[^" + y + "]*$";
    }
}
