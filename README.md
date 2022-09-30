# MatchRank

An alternative scoring system for round robin tournaments. Iterative algoroithm based on Google PageRank.
This has been designed to work on Round Robin Tennis doubles tournaments, but could be used for other contests.

## The Tournament

The existing tennis tournament is run as follows:
In a sample tournamant of 16 players, each player plays 4 matches. In each round, players are randomly assigned their playing partner and their 2 opponents.
Normal tennis scoring is not used. Instead each pair simply counts how many points they won. The winner is the player who has won most points across all 4 of their matches.
Each round goes on for a set time period, generally around 30 minutes.

### Sample realistic tournament result
```
Name            Match  Total    Won   Lost   Diff    Win%
--------------  -----  -----  -----  -----  -----  ------
Viraj               4    184    112     72     40  60.87%
Sam                 4    190    112     78     34  58.95%
Gary                4    194    112     82     30  57.73%
Andy                4    187    102     85     17  54.55%
Kehinde             4    153     93     60     33  60.78%
Heb                 4    192     93     99     -6  48.44%
Stew                4    206     88    118    -30  42.72%
Salima              4    184     80    104    -24  43.48%
Sundaram            4    160     79     81     -2  49.38%
Donna               4    163     74     89    -15  45.40%
Vikram              4    153     66     87    -21  43.14%
Cameron             4    170     57    113    -56  33.53%
```

The winner Viraj was decided on the number of points won. Points lost is then used as a tie breaker on equal players.
By not factoring overall win%, it disproportionally favours players who take on riskier shots, finishing points more quickly.

It also favours players who minimise time wasted between points.
Kehinde had the second highest win% overall, very close to 1st place. However, she finished 5th due to having played far fewer points than most others.

The scoring also takes no account of the relative strengths of teammates and opponents. 
The player pairings are fully randomised between rounds, and by chance some will be drawn against much stronger opponents or with weaker partners.

One final problem is that it is difficult to complete the tournament fairly, if one player has to drop out part way through, for injury or any other reason.

### Alternative algorithm

The alternative operates as follows:

* Each player is given a rating for each match they play. 1000 is considered average. Most match scores will fall between around 500 and 1500.
* A players final rating is the average of all matches they play.
* The match rating is based on the proportion of points won, against expectation based on the ratings of the other players in the match.
* Each team member gets an equal rating for any one match.
* Every player is initially assumed to be equal skill, with a rating of 1000. The algorithm is repeated for a number of generations. In each generation, the newest player ratings are used to calculate the match ratings.

### New tournament result (after 20 generations)

```
Name             Rating  Match  Total    Won   Lost   Diff    Win%
--------------  -------  -----  -----  -----  -----  -----  ------
Viraj           1142.35      4    184    112     72     40  60.87%
            Viraj(1142), Donna(906)  33 V 24  Andy(1082), Vikram(920)              Match Rating: 1144.63
            Viraj(1142), Gary(1125)  43 V 13  Salima(942), Stew(918)               Match Rating: 1397.76
           Viraj(1142), Salima(942)  21 V 21  Andy(1082), Stew(918)                Match Rating: 979.81
             Viraj(1142), Sam(1113)  15 V 14  Gary(1125), Vikram(920)              Match Rating: 986.20

Kehinde         1133.63      4    153     93     60     33  60.78%
       Kehinde(1134), Sundaram(985)  30 V 22  Salima(942), Cameron(748)            Match Rating: 1037.15
          Kehinde(1134), Donna(906)  16 V 19  Sundaram(985), Vikram(920)           Match Rating: 883.93
            Kehinde(1134), Heb(986)  23 V 9   Cameron(748), Vikram(920)            Match Rating: 1284.37
         Kehinde(1134), Salima(942)  24 V 10  Cameron(748), Donna(906)             Match Rating: 1268.55

Gary            1124.82      4    194    112     82     30  57.73%
              Gary(1125), Stew(918)  33 V 39  Sam(1113), Heb(986)                  Match Rating: 929.14
            Gary(1125), Viraj(1142)  43 V 13  Salima(942), Stew(918)               Match Rating: 1397.76
              Gary(1125), Sam(1113)  22 V 15  Sundaram(985), Donna(906)            Match Rating: 1097.10
            Gary(1125), Vikram(920)  14 V 15  Sam(1113), Viraj(1142)               Match Rating: 1015.23

Sam             1112.73      4    190    112     78     34  58.95%
                Sam(1113), Heb(986)  39 V 33  Gary(1125), Stew(918)                Match Rating: 1068.98
              Sam(1113), Andy(1082)  36 V 16  Cameron(748), Heb(986)               Match Rating: 1239.22
              Sam(1113), Gary(1125)  22 V 15  Sundaram(985), Donna(906)            Match Rating: 1097.10
             Sam(1113), Viraj(1142)  15 V 14  Gary(1125), Vikram(920)              Match Rating: 986.20

Andy            1082.01      4    187    102     85     17  54.55%
            Andy(1082), Vikram(920)  24 V 33  Donna(906), Viraj(1142)              Match Rating: 851.98
              Andy(1082), Sam(1113)  36 V 16  Cameron(748), Heb(986)               Match Rating: 1239.22
              Andy(1082), Stew(918)  21 V 21  Salima(942), Viraj(1142)             Match Rating: 1021.04
              Andy(1082), Stew(918)  21 V 15  Sundaram(985), Heb(986)              Match Rating: 1158.03

Heb              985.69      4    192     93     99     -6  48.44%
                Heb(986), Sam(1113)  39 V 33  Gary(1125), Stew(918)                Match Rating: 1068.98
             Heb(986), Cameron(748)  16 V 36  Andy(1082), Sam(1113)                Match Rating: 697.18
            Heb(986), Kehinde(1134)  23 V 9   Cameron(748), Vikram(920)            Match Rating: 1284.37
            Heb(986), Sundaram(985)  15 V 21  Stew(918), Andy(1082)                Match Rating: 839.59

Sundaram         984.69      4    160     79     81     -2  49.38%
       Sundaram(985), Kehinde(1134)  30 V 22  Salima(942), Cameron(748)            Match Rating: 1037.15
         Sundaram(985), Vikram(920)  19 V 16  Kehinde(1134), Donna(906)            Match Rating: 1124.33
          Sundaram(985), Donna(906)  15 V 22  Sam(1113), Gary(1125)                Match Rating: 885.10
            Sundaram(985), Heb(986)  15 V 21  Stew(918), Andy(1082)                Match Rating: 839.59

Salima           941.80      4    184     80    104    -24  43.48%
          Salima(942), Cameron(748)  22 V 30  Sundaram(985), Kehinde(1134)         Match Rating: 953.43
             Salima(942), Stew(918)  13 V 43  Gary(1125), Viraj(1142)              Match Rating: 515.13
           Salima(942), Viraj(1142)  21 V 21  Andy(1082), Stew(918)                Match Rating: 979.81
         Salima(942), Kehinde(1134)  24 V 10  Cameron(748), Donna(906)             Match Rating: 1268.55

Vikram           919.82      4    153     66     87    -21  43.14%
            Vikram(920), Andy(1082)  24 V 33  Donna(906), Viraj(1142)              Match Rating: 851.98
         Vikram(920), Sundaram(985)  19 V 16  Kehinde(1134), Donna(906)            Match Rating: 1124.33
          Vikram(920), Cameron(748)   9 V 23  Kehinde(1134), Heb(986)              Match Rating: 638.64
            Vikram(920), Gary(1125)  14 V 15  Sam(1113), Viraj(1142)               Match Rating: 1015.23

Stew             918.09      4    206     88    118    -30  42.72%
              Stew(918), Gary(1125)  33 V 39  Sam(1113), Heb(986)                  Match Rating: 929.14
             Stew(918), Salima(942)  13 V 43  Gary(1125), Viraj(1142)              Match Rating: 515.13
              Stew(918), Andy(1082)  21 V 21  Salima(942), Viraj(1142)             Match Rating: 1021.04
              Stew(918), Andy(1082)  21 V 15  Sundaram(985), Heb(986)              Match Rating: 1158.03

Donna            906.29      4    163     74     89    -15  45.40%
            Donna(906), Viraj(1142)  33 V 24  Andy(1082), Vikram(920)              Match Rating: 1144.63
          Donna(906), Kehinde(1134)  16 V 19  Sundaram(985), Vikram(920)           Match Rating: 883.93
          Donna(906), Sundaram(985)  15 V 22  Sam(1113), Gary(1125)                Match Rating: 885.10
           Donna(906), Cameron(748)  10 V 24  Kehinde(1134), Salima(942)           Match Rating: 663.09

Cameron          748.07      4    170     57    113    -56  33.53%
          Cameron(748), Salima(942)  22 V 30  Sundaram(985), Kehinde(1134)         Match Rating: 953.43
             Cameron(748), Heb(986)  16 V 36  Andy(1082), Sam(1113)                Match Rating: 697.18
          Cameron(748), Vikram(920)   9 V 23  Kehinde(1134), Heb(986)              Match Rating: 638.64
           Cameron(748), Donna(906)  10 V 24  Kehinde(1134), Salima(942)           Match Rating: 663.09
```
Kehinde has now moved up to second place. Largely the results are in line with the raw win%. However some people have moved up to 3 places, based on the strength of their teammates or opponents.

### Convergence

The aim is for the algorithm to err on under correcting for perceived player ratings. It is also acceptable to over correct if this is mitigated by running more iterations.
The intention is that each player rating will settle to a stable point after a reasonable number of iterations.

A report can be generated showing each player's rating after each generation.
```
Andy,1000.00,1098.35,1077.76,1080.64,1084.86,1078.87,1084.71,1079.84,1083.62,1080.81,1082.84,1081.40,1082.41,1081.71,1082.19,1081.86,1082.08,1081.93,1082.04,1081.97,1082.01
Cameron,1000.00,653.07,798.92,723.79,760.71,741.61,751.45,746.29,749.01,747.57,748.34,747.92,748.15,748.03,748.10,748.06,748.08,748.07,748.07,748.07,748.07
Donna,1000.00,867.81,918.65,903.42,905.78,907.70,904.86,907.49,905.38,906.96,905.82,906.63,906.07,906.45,906.19,906.37,906.25,906.33,906.27,906.31,906.29
Gary,1000.00,1151.77,1122.31,1120.82,1128.67,1121.87,1126.80,1123.51,1125.66,1124.27,1125.17,1124.58,1124.97,1124.72,1124.88,1124.77,1124.85,1124.80,1124.83,1124.81,1124.82
Heb,1000.00,992.39,972.32,996.85,977.58,991.31,981.93,988.23,984.02,986.83,984.95,986.21,985.37,985.93,985.55,985.81,985.64,985.75,985.67,985.72,985.69
Kehinde,1000.00,1229.35,1081.48,1163.86,1117.26,1142.87,1128.49,1136.58,1131.94,1134.64,1133.05,1134.00,1133.42,1133.78,1133.55,1133.70,1133.60,1133.66,1133.63,1133.65,1133.63
Salima,1000.00,930.55,942.47,943.09,940.60,942.63,941.24,942.18,941.55,941.97,941.69,941.88,941.75,941.84,941.78,941.82,941.79,941.81,941.80,941.81,941.80
Sam,1000.00,1172.91,1083.68,1127.13,1105.23,1116.67,1110.60,1113.90,1112.08,1113.10,1112.52,1112.86,1112.66,1112.78,1112.70,1112.75,1112.72,1112.74,1112.73,1112.73,1112.73
Stew,1000.00,886.90,930.73,909.67,923.74,913.93,921.08,915.93,919.60,917.01,918.81,917.57,918.42,917.84,918.24,917.97,918.15,918.03,918.11,918.05,918.09
Sundaram,1000.00,970.93,992.58,980.82,986.26,984.15,984.76,984.78,984.56,984.82,984.59,984.77,984.63,984.74,984.66,984.71,984.68,984.70,984.69,984.70,984.69
Vikram,1000.00,863.96,955.20,898.14,933.42,911.45,925.06,916.51,921.91,918.46,920.68,919.24,920.18,919.56,919.97,919.70,919.88,919.76,919.84,919.79,919.82
Viraj,1000.00,1182.02,1123.89,1151.77,1135.89,1146.94,1139.03,1144.76,1140.66,1143.56,1141.54,1142.93,1141.97,1142.63,1142.18,1142.48,1142.28,1142.42,1142.33,1142.39,1142.35
```

After running a single generation Kehinde (1229.35) is on top of the ratings and Cameron (653.07) is at the bottom. These 2 players played against each other 3 times out of 4, so their ratings have an inverse effect on the other.
In generation 2 Kehinde (1081.48) drops and Cameron (798.92) rises.
In generation 3 Kehinde (1163.86) bounces back up and Cameron (723.79) down, though not as far as generation 1.
Every generation converges better approximation of the final ratings. After around 15-20 generations there is little movement in any player ratings. 

## Next Steps

Prove that this algorithm will converge to a stable point for any data set. How?
Assess different approached to individual match ratings


