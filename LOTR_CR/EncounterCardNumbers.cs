﻿namespace LOTR_CR;

public static class EncounterCardNumbers
{
  public static Dictionary<(int, int), int> NumbersPerCollection { get; private set; } = new()
  {
    // Les Craintes de l'Intendant 
    { (16, 14), 5 },
    { (16, 15), 1 },
    { (16, 16), 1 },
    { (16, 17), 1 },
    { (16, 18), 3 },
    { (16, 19), 1 },
    { (16, 20), 2 },
    { (16, 21), 1 },
    { (16, 22), 1 },
    { (16, 23), 2 },
    { (16, 24), 3 },
    { (16, 25), 2 },
    { (16, 26), 1 },
    { (16, 27), 1 },
    { (16, 28), 1 },
    { (16, 29), 1 },
    { (16, 30), 1 },
    { (16, 31), 1 },
    // La Forêt de Druadan
    { (17, 45), 1 },
    { (17, 46), 3 },
    { (17, 47), 3 },
    { (17, 48), 4 },
    { (17, 49), 3 },
    { (17, 50), 1 },
    { (17, 51), 3 },
    { (17, 52), 3 },
    { (17, 53), 3 },
    { (17, 54), 3 },
    { (17, 55), 2 },
    // Rencontre à Amon Din
    { (18, 68), 1 },
    { (18, 69), 1 },
    { (18, 70), 1 },
    { (18, 71), 1 },
    { (18, 72), 1 },
    { (18, 73), 1 },
    { (18, 74), 2 },
    { (18, 75), 4 },
    { (18, 76), 4 },
    { (18, 77), 3 },
    { (18, 78), 2 },
    { (18, 79), 2 },
    { (18, 80), 2 },
    // Assaut sur Osgiliath
    { (19, 92), 2 },
    { (19, 93), 2 },
    { (19, 94), 2 },
    { (19, 95), 2 },
    { (19, 96), 1 },
    { (19, 97), 1 },
    { (19, 98), 1 },
    { (19, 99), 1 },
    { (19, 100), 2 },
    { (19, 101), 2 },
    { (19, 102), 3 },
    { (19, 103), 3 },
    { (19, 104), 2 },
    { (19, 105), 2 },
    { (19, 106), 2 },
    // Le Sang du Gondor
    { (20, 119), 1 },
    { (20, 120), 4 },
    { (20, 121), 3 },
    { (20, 122), 2 },
    { (20, 123), 1 },
    { (20, 124), 1 },
    { (20, 125), 1 },
    { (20, 126), 1 },
    { (20, 127), 1 },
    { (20, 128), 4 },
    { (20, 129), 4 },
    { (20, 130), 3 },
    { (20, 131), 2 },
    { (20, 132), 1 },
    { (20, 133), 1 },
    // La Vallée de Morgul
    { (21, 147), 1 },
    { (21, 148), 3 },
    { (21, 149), 2 },
    { (21, 150), 3 },
    { (21, 151), 1 },
    { (21, 152), 1 },
    { (21, 153), 1 },
    { (21, 154), 3 },
    { (21, 155), 2 },
    { (21, 156), 1 },
    { (21, 157), 2 },
    { (21, 158), 2 },
    { (21, 159), 2 },
    { (21, 160), 2 },
    // Par Monts et par Souterrains
    { (43, 32), 1 },
    { (43, 33), 1 },
    { (43, 34), 1 },
    { (43, 35), 3 },
    { (43, 36), 3 },
    { (43, 37), 1 },
    { (43, 38), 2 },
    { (43, 39), 2 },
    { (43, 40), 2 },
    { (43, 41), 2 },
    { (43, 42), 1 },
    { (43, 43), 1 },
    { (43, 44), 1 },
    { (43, 45), 1 },
    { (43, 46), 1 },
    { (43, 47), 1 },
    { (43, 48), 1 },
    { (43, 49), 1 },
    { (43, 50), 1 },
    { (43, 51), 3 },
    { (43, 52), 2 },
    { (43, 53), 3 },
    { (43, 54), 3 },
    { (43, 55), 2 },
    { (43, 56), 2 },
    { (43, 57), 1 },
    { (43, 58), 3 },
    { (43, 59), 3 },
    { (43, 60), 3 },
    { (43, 61), 3 },
    { (43, 62), 4 },
    { (43, 63), 1 },
    { (43, 64), 3 },
    { (43, 65), 3 },
    { (43, 66), 2 },
    { (43, 67), 3 },
    { (43, 68), 2 },
    { (43, 69), 1 },
    { (43, 70), 3 },
    { (43, 71), 2 },
    { (43, 72), 4 },
    { (43, 73), 2 },
    { (43, 74), 1 },
    { (43, 75), 1 },
    { (43, 76), 4 },
    { (43, 77), 3 },
    { (43, 78), 1 },
    { (43, 79), 1 },
    { (43, 80), 2 },
    { (43, 81), 3 },
    { (43, 82), 2 },
    { (43, 83), 2 },
    { (43, 84), 1 },
    // Au Seuil de la Porte
    { (44, 25), 1 },
    { (44, 26), 4 },
    { (44, 27), 2 },
    { (44, 28), 3 },
    { (44, 29), 3 },
    { (44, 30), 3 },
    { (44, 31), 4 },
    { (44, 32), 5 },
    { (44, 33), 2 },
    { (44, 34), 2 },
    { (44, 35), 3 },
    { (44, 36), 3 },
    { (44, 37), 2 },
    { (44, 38), 2 },
    { (44, 39), 1 },
    { (44, 40), 1 },
    { (44, 41), 1 },
    { (44, 42), 3 },
    { (44, 43), 1 },
    { (44, 44), 3 },
    { (44, 45), 2 },
    { (44, 46), 3 },
    { (44, 47), 3 },
    { (44, 48), 3 },
    { (44, 49), 2 },
    { (44, 50), 4 },
    { (44, 51), 2 },
    { (44, 52), 1 },
    { (44, 53), 1 },
    { (44, 54), 4 },
    { (44, 55), 3 },
    { (44, 56), 4 },
    { (44, 57), 4 },
    { (44, 58), 2 },
    { (44, 59), 3 },
    { (44, 60), 3 },
    { (44, 61), 1 },
    { (44, 62), 1 },
    { (44, 63), 1 },
    { (44, 64), 2 },
    { (44, 65), 2 },
    { (44, 66), 2 },
  }; 
}
