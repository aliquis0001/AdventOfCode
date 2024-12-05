import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class Day1 {
    public static void main(String[] args) {
        long start = System.currentTimeMillis();
        Scanner inFile = null;
        try {   //Create Scanner from file
            inFile = new Scanner(new File("input1.txt"));
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
        int[] data = new int[200]; //hardcoding because i'm lazy
        for (int i = 0; i < 200; i++) {
            data[i] = inFile.nextInt(); //get the data in the program, the easy way
        }
        System.out.println(System.currentTimeMillis() - start);
        long created = System.currentTimeMillis();
        for (int i = 0; i < 199; i++) { //nested for loops because it's the first thing i thought of and i don't like thinking of several things
            for (int j = i + 1; j < 200; j++) {
                if (data[i] + data[j] == 2020) {
                    System.out.println(data[i] * data[j]);
                    System.out.println(System.currentTimeMillis() - created);
                }
            }
        }
        long two = System.currentTimeMillis();
        for (int i = 0; i < 198; i++) {
            for (int j = i + 1; j < 199; j++) {
                for (int k = j + 1; k < 200; k++) {
                    if (data[i] + data[j] + data[k] == 2020) {
                        System.out.println(data[i] * data[j] * data[k]);
                        System.out.println(System.currentTimeMillis() - two);
                    }
                }
            }
        }
    }
}
