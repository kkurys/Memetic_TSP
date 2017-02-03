# Memetic_TSP
Memetic algorithm to solve TSP problem with constraints
- Problem
Given N cities located on coordinates (float numbers)  with P profit each, find route with highest summary profit and shorter than given
L max distance.
Input:
N (1 <= N <= 400) L (1 <= L <= 10000)
N lines:
X Y P
starting with capital with profit = 0
Output:
profit
path (numbers of towns separated by spaces)

- Algorithm description: 
  - Dictionary:
    - Town:
      Represented by integer and profit. Located on 2D map on coordinates given in input file.
    - Profit
      Value of a town.
    - Individual:
      Single path represented by list of towns it goes through.
    - Individual Fitness:
      Factor that indicates how good is the individual. It's measured by multiplying profit and distance and dividing by maximum distance raised to power.
    - Modifications (mutations):
      There are 6 types of modification:
      - removing part of road and trying to fill it up till the limit
      - exchanging random town for an unused one with best profit
      - exchanging part of route for a new one
      - moving parts of route
      - exchanging random towns for random others
      - 2-opt (or rather: partial 2-opt since it's going only for a few iterations)
    - Evaluation:
      Each newly created individual is compared to his "parents" and then compared to the one more similiar to it. There's a chance that parent will win over a newly created individual.
    - Population:
      Set of individuals.
  - How it works:
    In the beginning an initial population is generated by taking random city from 2 to 4 (at random) best cities for the next step and then inserting the capital if needed. Then over the generations it's individuals are crossed with a probability. Each newly generated individual is mutated and evaluated.
    While the algorithm works it keeps track of the best fitting individual that was produced, which ensures that even if it falls out of the population the algorithm produces the best result.
  - Comment:
  My first attempt at genetic algorithms. However, experiments with modification algorithms and frequency resulted in heading more towards memetic
  algorithm since stronger modifications and less number of generations gave better results.
  The code could have been written better, especially the modification methods.
  Algorithm was parametrized by hand based on experience, which also means that it's parameters may not be optimal or even sub-optimal.