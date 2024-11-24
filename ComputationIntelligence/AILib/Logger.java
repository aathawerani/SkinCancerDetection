/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ciproject.AILib;

import java.lang.String;
/**
 *
 * @author aaht14
 */
public class Logger {
    
    int _LogLevel;
    
    public Logger(int level)
    {
        _LogLevel = level;
    }
    
    public void Debug(String message)
    {
        if(_LogLevel == Utility.LogDebug)
        {
            System.out.println(message);
        }
    }

    public void Debug(double[] array)
    {
        if(_LogLevel == Utility.LogDebug)
        {
            for(int i=0; i<array.length; i++)
            {
                System.out.print(" " + array[i]);
            }
            System.out.println();
        }
    }

    public void Info(double[] array)
    {
        for(int i=0; i<array.length; i++)
        {
            System.out.print(" " + array[i]);
        }
        System.out.println();
    }

    public void Info(int[] array)
    {
        for(int i=0; i<array.length; i++)
        {
            System.out.print(" " + array[i]);
        }
        System.out.println();
    }

    public void Debug(char[] array)
    {
        if(_LogLevel == Utility.LogDebug)
        {
            for(int i=0; i<array.length; i++)
            {
                System.out.print(" " + array[i]);
            }
            System.out.println();
        }
    }
    
    public void Debug(int[] array)
    {
        if(_LogLevel == Utility.LogDebug)
        {
            for(int i=0; i<array.length; i++)
            {
                System.out.print(" " + array[i]);
            }
            System.out.println();
        }
    }

    public void Debug(String[] array)
    {
        if(_LogLevel == Utility.LogDebug)
        {
            for(int i=0; i<array.length; i++)
            {
                System.out.print(" " + array[i]);
            }
            System.out.println();
        }
    }
    
    public void Info(String[] array)
    {
        for(int i=0; i<array.length; i++)
        {
            System.out.print(" " + array[i]);
        }
        System.out.println();

    }

    public void Info(String message)
    {
        System.out.println(message);
    }

    public void Error(String message)
    {
        if(_LogLevel == Utility.LogError)
        {
            System.out.println(message);
        }
    }
}
