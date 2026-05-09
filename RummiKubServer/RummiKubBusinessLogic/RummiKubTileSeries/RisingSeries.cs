using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RummiKubClasses.Tiles
{
    //[DataContract]

    public class RisingSeries : TileSeries
    {
        public RisingSeries()
        {

        }
        public RisingSeries(List<Tile> tiles) : base(tiles)
        {
        }
        public RisingSeries(List<Tile> tiles, int seriesId) : base(seriesId, tiles)
        {
        }
        public override void CreateSeries(List<Tile> tiles)
        {
            if (!IsValid(tiles))
            {
                throw new Exception("Cannot Make a Rising Series");
            }

        }
        //

        public override bool IsValid(List<Tile> tiles) //sorts and checks if the series is valid for rising series
        {

            if (tiles.Count < 3)
                return false;
            int jokers = ResetJokerValues(tiles);//resets all jokers and gets the count of all the jokers in the series
            base.set = tiles.OrderBy(a => a.Value).ToList();
            int mistakes = 0;
            Stack<Tuple<int, int>> indexs_and_values_for_jokers = new Stack<Tuple<int, int>>();
            //store them in a stack so when they will
            for (int i = 0; i < set.Count - 1; i++)
            {
                if (set[i] is Joker)
                    continue;
                if (set[i].Value == set[i + 1].Value)//two of the exact same value
                {
                    return false;
                }
                if (set[i].Color != set[i + 1].Color)//two different colors
                    return false;

                if (set[i].Value != set[i + 1].Value - 1)// if current tile isnt minus 1 from the next
                {
                    for (int missingval = set[i].Value + 1; missingval < set[i + 1].Value; missingval++)
                    {
                        mistakes++;//gets the range of missing numbers between them
                        indexs_and_values_for_jokers.Push(new Tuple<int, int>(i + 1, missingval));// insert the wanted index and value for the joker 
                    }
                }

            }
            while(indexs_and_values_for_jokers.Count!=0)// replaces the missing tile with a joker
            {
                Tuple<int,int> index_and_value = indexs_and_values_for_jokers.Pop();
                if (jokers == 0)
                    break;
                set.Insert(index_and_value.Item1, set[0]);
                set[0].Value = index_and_value.Item2;// assign the new joker "fake value"
                set.RemoveAt(0);
                mistakes--;
                jokers--;
            }
            while (jokers > 0)//adds the joker in the end if there arent any mistakes
            {
                if (GetMax() != 13)
                {
                    set[0].Value = GetMax() + 1;
                    set.Add(set[0]);
                }
                else
                {
                    set[0].Value = GetMin() - 1;
                    set.Insert(jokers, set[0]); //inserts it after all the unsigned jokers if the value 13 is already in the series
                }
                set.RemoveAt(0);
                jokers--;

            }
            return !(mistakes > 0);//if there are still missing tiles, the series isnt valid
        }
        private int ResetJokerValues(List<Tile> tiles)//resets the value of the joker from his past series
        {
            int JokerCount = 0;
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i] is Joker)
                {
                    JokerCount++;
                    tiles[i].Value = 0;
                }
            }
            return JokerCount;
        }
        private int GetMax()//get the highest value in the set
        {
            return this.set[this.set.Count - 1].Value;
        }
        private int GetMin()//get the lowest non unasigned joker value
        {
            return this.set[0].Value;
        }
        public override void TryAddTile(Tile addedtile)
        {
            bool check = false;
            //if (addedtile is Joker) // add at end and sort it in the right place
            //{
            //    set.Add(addedtile);
            //    addedtile.Value = GetMax() + 1;
            //    IsValid(set);
            //    check = true;

            //}
             if (addedtile.Value > GetMax() && !(addedtile is Joker))
            {
                set.Add(addedtile);//Add at end
                check = true;
            }
            else if (addedtile.Value < GetMin() && !(addedtile is Joker))
            {
                set.Insert(0, addedtile); //Add at start
                check = true;
            }
            if (!check)
            {
                for (int i = 1; i < base.set.Count; i++)
                {
                    //if (base.set[i].Value == addedtile.Value)// 3 {1,2,3,4,5}->{1,2,3,3,4,5}
                    //{
                    //    check = true;
                    //    base.set.Insert(i, addedtile);
                    //    break;
                    //}
                     if ( base.set[i - 1].Value <= addedtile.Value && base.set[i].Value >= addedtile.Value)
                    { // 4 {2,3,5}->{2,3,4,5}, {1,2,5}->{1,2,4,5}, {2,6}->{2,4,6},{2,3,4,5}->{2,3,4,4,5}
                        check = true;
                        base.set.Insert(i, addedtile);
                        break;
                    }
                    else if(addedtile is Joker && base.set[i].Value - base.set[i - 1].Value > 1)
                    {//j {2,4}->{2,j,4},{1,2,5}->{1,2,j,5}
                        check = true;
                    }

                }
            }
            if (!check)
            {
                if (GetMax() != 13)
                {
                    set.Add(addedtile);
                    addedtile.Value = GetMax() + 1;
                    check = true;

                }
                else
                {
                    addedtile.Value = GetMin() - 1;
                    set.Insert(0,addedtile);
                    check = true;

                }

            }
        }
    }

}

