/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package ciproject;

import java.io.*;
import ciproject.AILib.*;
import ciproject.BaseCode.*;
import ciproject.Problems.*;
import ciproject.Algos.*;
import smile.Network;

/**
 *
 * @author aaht14
 */
public class CIProject {

    static double[][] GenerationResults;
    static double[][] RunsResults;
    static double[][] RunsResults2;

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) throws IOException {

        //Assignment1();
        //Assignment3();
        
        StructureLearning();
    }
    
    public static void StructureLearning() throws IOException
    {
        Logger log = new Logger(Utility.LogInfo);
        
        //String filepath1 = "Skin2";
        String filepath1 = "Skin2";
        String filepath2 = "SKIN11";
        
        FileIO fio = new FileIO(filepath1, filepath2, log);
        
        ReadGenieData RGD = new ReadGenieData(fio, log);
        
        int maxval = 5;
        int maxrows = 10000;
        
        RGD.ReadHeader(maxval);
        RGD.ReadData(maxrows);
        RGD.ReadEntireData(maxrows);
        
        String[][] values = RGD.GetValues();
        int[][] valuecounts = RGD.GetValueCount();
        int nodescount = RGD.GetNumberOfNodes();
        
        log.Debug("nodescount " + nodescount);
        
        for(int i=0; i<nodescount; i++)
        {
            log.Debug(values[i]);
            log.Debug(valuecounts[i]);
        }
        
        double[] minx, maxx, miny, maxy;
        double stepx = 0.1, stepy = 0.1;
        int valuetype, elementdepth, chromosomesize;
        
        elementdepth = chromosomesize = RGD.GetNumberOfNodes();
        valuetype = Utility.UseDouble;
        minx = new double[chromosomesize];
        maxx = new double[chromosomesize];
        miny = new double[chromosomesize]; 
        maxy = new double[chromosomesize];
        
        int[][] valuecount = RGD.GetValueCount();
        
        for(int i=0; i<elementdepth; i++)
        {
            minx[i] = 0;
            maxx[i] = 1;
            miny[i] = 0;
            maxy[i] = 1;
        }
        
        log.Debug(minx); log.Debug(maxx);

        SLPopulationGenerator SLPG = new SLPopulationGenerator(minx, maxx, miny, maxy, stepx, stepy,
        valuetype, elementdepth, chromosomesize, log);
        
        //log=new Logger(Utility.LogDebug);
        
        SLAlgoConfig SLAC = new SLAlgoConfig(log); 
        
        int PopulationSize = 100, Offsprings = 100, Iterations = 1000; SLAC._MaxRank=21; SLAC._TParentSampleSize=5; 
        //int PopulationSize = 5, Offsprings = 5, Iterations = 1; SLAC._MaxRank=3; SLAC._TParentSampleSize=3;
        
        
        SLAC._ParentSelectionScheme=Utility.TournamentSelection; 
        SLAC._SurvivorSelectionScheme=Utility.RankBasedSurvival; SLAC._FitnessSelectionParameter=5;
        SLAC._RankSelectionParameter=1.5; 
        
        SLAC._CrossOverRate=0.7; SLAC._CrossOverScheme=Utility.RandomCrossOver; 
        SLAC._DegreeOfMutation=1; SLAC._FitnessProportionate=Utility.MutateEvenly; 
        SLAC._MutationRate=0.7; SLAC._MutationVariationPercentage=100; 
        SLAC._MutationScheme=Utility.RandomMutation; 

        SLAC._ProblemType=Utility.Maximization; SLAC._PopulationCount=PopulationSize; SLAC._ChromosomeLength=chromosomesize; 
        SLAC._NumberOfOffsprings=Offsprings; 
        
        //SLAC._TotalTrials = RGD.GetTotalTrials(); SLAC._PriorTrials=1; 
        SLAC._ValueCount=valuecount;
        SLAC._InputData = RGD._InputData; SLAC._Values = values;
        
        //log = new Logger(Utility.LogDebug);
        EAlgorithm2 SLAlgorithm = new EAlgorithm2(log, SLPG, SLAC); 

        int max = 0, min = 0;
        
        for(int i=0; i<Iterations; i++)
        {
            log.Info("Iteration " + i);
            SLAlgorithm.Execute();
            max = SLAlgorithm.BestFitness();
            min = SLAlgorithm.WorstFitness();
            double fitness = SLAlgorithm.Fitness(max);
            double fitness2 = SLAlgorithm.Fitness(min);
            log.Info("max " + max + " fitness " + fitness + " worst " + min + " fitness " + fitness2);
        }
        
        //log = new Logger(Utility.LogInfo);
        
        Network net = new Network();
        
        for(int a=0; a<chromosomesize; a++)
        {
            net.addNode(Network.NodeType.Cpt, RGD._Header[a]);
        }
        
        int[][] graphpaths = SLAC.GetGraphPath(max);
        int length = graphpaths.length;
        for(int i=0; i<length; i++)
        {
            if(graphpaths[i][0] == -1) break;
            //log.Info("graphpaths " + RGD._Header[i]);
            int depth = graphpaths[i].length;
            String path = "";
            for(int j=0; j<depth; j++)
            {
                if(graphpaths[i][j+1] == -1) break;
                path += RGD._Header[graphpaths[i][j]] + ", ";
                //log.Debug("parents " + path);
                try{
                    net.addArc(RGD._Header[graphpaths[i][j]], RGD._Header[graphpaths[i][j+1]]);
                }
                catch(java.lang.Exception e)
                {
                    net.addArc(RGD._Header[graphpaths[i][j+1]], RGD._Header[graphpaths[i][j]]);
                }
            }
            log.Info(path);
            //log.Info(graphpath[i]);
        }
        net.writeFile("results.xdsl");
        

        
        
        Network net2 = new Network();
        
        for(int a=0; a<chromosomesize; a++)
        {
            net2.addNode(Network.NodeType.Cpt, RGD._Header[a]);
        }
        
        int[][] graphpaths2 = SLAC.GetGraphPath(min);
        int length2 = graphpaths2.length;
        for(int i=0; i<length2; i++)
        {
            if(graphpaths2[i][0] == -1) break;
            //log.Info("parents2 " + RGD._Header[i]);
            int depth = graphpaths2[i].length;
            String path = "";
            for(int j=0; j<depth; j++)
            {
                if(graphpaths2[i][j+1] == -1) break;
                path += RGD._Header[graphpaths2[i][j]] + ", ";
                //log.Debug("parents " + path);
                try
                {
                    net2.addArc(RGD._Header[graphpaths2[i][j]], RGD._Header[graphpaths2[i][j+1]]);
                }
                catch(java.lang.Exception e)
                {
                    net2.addArc(RGD._Header[graphpaths2[i][j+1]], RGD._Header[graphpaths2[i][j]]);
                }
            }
            log.Info(path);
            //log.Info(graphpath[i]);
        }
        net2.writeFile("results2.xdsl");
    }
    
    public static void Assignment1()
    {
        int PopulationSize = 10, Offsprings = 10, Iterations = 40, Runs = 10;
        Logger log = new Logger(Utility.LogDebug);
        double minx, maxx, miny, maxy, stepx, stepy;
        P1PopulationGenerator P1PG;
        EAProblem1 EAP1; EAProblem2 EAP2; EAProblem3 EAP3; 
        EAlgorithm Problem1, Problem2, Problem3;

        //FPS and Truncation
        
/*        
        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP1._FitnessSelectionParameter=2;
        EAP1._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP2._FitnessSelectionParameter=2;
        EAP2._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP3._FitnessSelectionParameter=2;
        EAP3._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);
    
        //RBS and Truncation
        
        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP1._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        EAP1._RankSelectionParameter=1.5; EAP1._MaxRank=3; EAP1._FitnessSelectionParameter=2;
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        
        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP2._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        EAP2._RankSelectionParameter=1.5; EAP2._MaxRank=3; EAP2._FitnessSelectionParameter=2;
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; 
        EAP3._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);

        
        //BT and Truncation
        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.TournamentSelection; 
        EAP1._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;
        EAP1._TParentSampleSize=2;EAP1._MaxRank=2; EAP1._RankSelectionParameter=1.5;
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        
        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.TournamentSelection; 
        EAP2._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;
        EAP2._TParentSampleSize=2;EAP2._MaxRank=2; EAP2._RankSelectionParameter=1.5;
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.TournamentSelection; 
        EAP3._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;
        EAP3._TParentSampleSize=2;EAP3._MaxRank=2; EAP3._RankSelectionParameter=1.5;
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);
        
        
        //FPS and BT

        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP1._FitnessSelectionParameter=2;
        EAP1._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  
        EAP1._TParentSampleSize=2;EAP1._MaxRank=2; EAP1._RankSelectionParameter=1.5;
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);


        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP2._FitnessSelectionParameter=2;
        EAP2._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  
        EAP2._TParentSampleSize=2;EAP2._MaxRank=2; EAP2._RankSelectionParameter=1.5;
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);



        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP3._FitnessSelectionParameter=2;
        EAP3._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  
        EAP3._TParentSampleSize=2;EAP3._MaxRank=2; EAP3._RankSelectionParameter=1.5;
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);
    
        //RBS and BT

        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP1._RankSelectionParameter=1.5; EAP1._MaxRank=3; EAP1._FitnessSelectionParameter=2;
        EAP1._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP1._TParentSampleSize=2;
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        
        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP2._RankSelectionParameter=1.5; EAP2._MaxRank=3; EAP2._FitnessSelectionParameter=2;
        EAP2._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP2._TParentSampleSize=2;
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP3._RankSelectionParameter=1.5; EAP3._MaxRank=3; EAP3._FitnessSelectionParameter=2;
        EAP3._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP3._TParentSampleSize=2;
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);

        //BT and BT
        
        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=10; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateEvenly;
        EAP1._ParentSelectionScheme=AIAlgos.TournamentSelection; 
        EAP1._RankSelectionParameter=1.5; EAP1._MaxRank=3; EAP1._FitnessSelectionParameter=2;
        EAP1._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP1._TParentSampleSize=2;
        Problem1 = new EAlgorithm(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        
        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=10; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateEvenly;
        EAP2._ParentSelectionScheme=AIAlgos.TournamentSelection; 
        EAP2._RankSelectionParameter=1.5; EAP2._MaxRank=3; EAP2._FitnessSelectionParameter=2;
        EAP2._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP2._TParentSampleSize=2;
        Problem2 = new EAlgorithm(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=10; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateEvenly;
        EAP3._ParentSelectionScheme=AIAlgos.RankBasedSelection; 
        EAP3._RankSelectionParameter=1.5; EAP3._MaxRank=3; EAP3._FitnessSelectionParameter=2;
        EAP3._SurvivorSelectionScheme=AIAlgos.TournamentSurvival;  EAP3._TParentSampleSize=2;
        Problem3 = new EAlgorithm(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);
*/
    }

    public static void Assignment3()
    {
        int PopulationSize = 10, Offsprings = 10, Iterations = 40, Runs = 10;
        Logger log = new Logger(Utility.LogDebug);
        double minx, maxx, miny, maxy, stepx, stepy;
        P1PopulationGenerator P1PG;
        EAProblem1 EAP1; EAProblem2 EAP2; EAProblem3 EAP3; 
        AIS Problem1, Problem2, Problem3;
/*
        minx = -5; maxx = 0; miny = 0; maxy = 5; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP1 = new EAProblem1(); EAP1.Initialize(log);
        EAP1._ChromosomeLength=1; EAP1._NumberOfOffsprings=2; EAP1._PopulationCount=10; 
        EAP1._ProblemType=Utility.Minimization; EAP1._DegreeOfMutation=1; 
        EAP1._FitnessProportionate=Population.MutateIncreasing; EAP1._MutationVariationPercentage=80;
        EAP1._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP1._FitnessSelectionParameter=2;
        EAP1._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        EAP1._NumberOfClones=2;
        Problem1 = new AIS(log, P1PG, EAP1);
        Execute(Problem1, EAP1, Runs, Iterations);

        
        minx = -2; maxx = 2; miny = -1; maxy = 3; stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP2 = new EAProblem2(); EAP2.Initialize(log);
        EAP2._ChromosomeLength=1; EAP2._NumberOfOffsprings=2; EAP2._PopulationCount=10; 
        EAP2._ProblemType=Utility.Maximization; EAP2._DegreeOfMutation=1; 
        EAP2._FitnessProportionate=Population.MutateIncreasing; EAP2._MutationVariationPercentage=80;
        EAP2._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP2._FitnessSelectionParameter=2;
        EAP2._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        EAP2._NumberOfClones=2;
        Problem2 = new AIS(log, P1PG, EAP2);
        Execute(Problem2, EAP2, Runs, Iterations);
        

        minx = Utility.DoubleMin; maxx = Utility.DoubleMax; miny = Utility.DoubleMin; maxy = Utility.DoubleMax; 
        stepx = 0.25; stepy = 0.25;
        P1PG = new P1PopulationGenerator(minx, maxx, miny, maxy, stepx, stepy);
        EAP3 = new EAProblem3(); EAP3.Initialize(log);
        EAP3._ChromosomeLength=1; EAP3._NumberOfOffsprings=2; EAP3._PopulationCount=10; 
        EAP3._ProblemType=Utility.Maximization; EAP3._DegreeOfMutation=1; 
        EAP3._FitnessProportionate=Population.MutateIncreasing; EAP3._MutationVariationPercentage=80;
        EAP3._ParentSelectionScheme=AIAlgos.FitnessProportionSelection; EAP3._FitnessSelectionParameter=2;
        EAP3._SurvivorSelectionScheme=AIAlgos.TruncationSurvival;  
        EAP3._NumberOfClones=2;
        Problem3 = new AIS(log, P1PG, EAP3);
        Execute(Problem3, EAP3, Runs, Iterations);
*/        
    }
    
    public static void Execute(AIAlgos Problem, AlgoConfig AC, int Runs, int Iterations)
    {
        GenerationResults = new double[2][Iterations];
        RunsResults = new double [Runs][Iterations];
        RunsResults2 = new double [Runs][Iterations];
        
        int i=0;
        for(int j=0; j<Runs; j++)
        {
            for(i=0; i<Iterations; i++)
            {
                Problem.Execute();
                SaveResult(Problem, AC, j, i);
            }
            PrintResults(Iterations);
        }
        PrintResults(Runs, Iterations);
    }

    protected static void SaveResult(AIAlgos Problem, AlgoConfig AC, int Runs, int Iteration)
    {
        int[] sorted = new int[AC._PopulationCount];
        double[] fitness = new double[AC._PopulationCount];
        fitness = Problem.GetGenerationFitness();
        Utility.Sort(fitness, sorted, AC._PopulationCount, AC._ProblemType);
        GenerationResults[0][Iteration] = fitness[sorted[0]];
        GenerationResults[1][Iteration] = Utility.Sum(fitness) / AC._PopulationCount;
        RunsResults[Runs][Iteration] = fitness[sorted[0]];
        RunsResults2[Runs][Iteration] = Utility.Sum(fitness) / AC._PopulationCount;
    }
    
    protected static void PrintResults(int Iterations)
    {
        System.out.println("Gen\t\t\t BF\t\t\t\t AF");
        for(int i=0; i<Iterations; i++)
        {
            System.out.println(" "+ i +"\t\t "+ GenerationResults[0][i] +"\t\t "+ GenerationResults[1][i]);
        }
            System.out.println();
    }

    protected static void PrintResults(int Runs, int Iterations)
    {
        System.out.println("Gen\t\t\t R1BSF\t\t\t\t R2BSF\t\t\t\t R3BSF\t\t\t\t R4BSF\t\t\t\t R5BSF\t\t\t\t "
                + "R6BSF\t\t\t\t R7BSF\t\t\t\t R8BSF\t\t\t\t R9BSF\t\t\t\t R10BSF\t\t\t\t ABSF");
        for(int j=0; j<Iterations; j++)
        {
            double average = 0;
            for(int i=0; i<Runs; i++)
            {
                average = average + RunsResults[i][j];
            }
            average = average / Runs;
            System.out.println(j + "\t\t " + RunsResults[0][j] + "\t\t " + RunsResults[1][j] + "\t\t " + RunsResults[2][j]
             + "\t\t " + RunsResults[3][j] + "\t\t " + RunsResults[4][j] + "\t\t " + RunsResults[5][j]
             + "\t\t " + RunsResults[6][j] + "\t\t " + RunsResults[7][j] + "\t\t " + RunsResults[8][j]
             + "\t\t " + RunsResults[9][j] + "\t\t " + average);
        }
            System.out.println();

        System.out.println("Gen\t\t\t R1ASF\t\t\t\t R2ASF\t\t\t\t R3ASF\t\t\t\t R4ASF\t\t\t\t R5ASF\t\t\t\t "
                + "R6ASF\t\t\t\t R7ASF\t\t\t\t R8ASF\t\t\t\t R9ASF\t\t\t\t R10ASF\t\t\t\t AASF");
        for(int j=0; j<Iterations; j++)
        {
            double average = 0;
            for(int i=0; i<Runs; i++)
            {
                average = average + RunsResults2[i][j];
            }
            average = average / Runs;
            System.out.println(j + "\t\t " + RunsResults2[0][j] + "\t\t " + RunsResults2[1][j] + "\t\t " + RunsResults2[2][j]
             + "\t\t " + RunsResults2[3][j] + "\t\t " + RunsResults2[4][j] + "\t\t " + RunsResults2[5][j]
             + "\t\t " + RunsResults2[6][j] + "\t\t " + RunsResults2[7][j] + "\t\t " + RunsResults2[8][j]
             + "\t\t " + RunsResults2[9][j] + "\t\t " + average);
        }
    }
}
