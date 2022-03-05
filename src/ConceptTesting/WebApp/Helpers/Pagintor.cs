using System;
using System.Collections;
using System.Collections.Generic;

namespace WebApp.Helpers
{
    public record PageState(int CurrentPage, int PageSize);
    public record PageRef(int Page, string Text);
    public class Paginator : IEnumerable<PageRef>
    {

        #region Public Fields
        public readonly int TotalItemCount;
        public readonly PageState CurrentState;
        public readonly PageRef CurrentPage;
        private List<PageRef> PageReferences;
        public string FirstPageText = "<First>";
        public string LastPageText = "<Last>";
        public string NextPageText = "<Next>";
        public string PreviousPageText = "<Prev>";
        #endregion

        #region Public properties with calculated Getters
        ///<summary>PageCount is the total number of pages for the TotalResults</summary>
        public int PageCount { get { return (TotalItemCount / CurrentState.PageSize) + 1; } }

        ///<summary>FirstPage is the human-friendly page number for the first page</summary>
        public int FirstPage { get { return 1; } }

        ///<summary>LastPage is the human-friendly page number for the last page</summary>
        public int LastPage { get { return PageCount; } }

        ///<summary>NextPage is the human-friendly page number for the next available page</summary>
        public int NextPage { get { return CurrentState.CurrentPage < LastPage ? CurrentState.CurrentPage + 1 : LastPage; } }

        ///<summary>PreviousPage is the human-friendly page number for the next available page</summary>
        public int PreviousPage { get { return CurrentState.CurrentPage > FirstPage ? CurrentState.CurrentPage - 1 : FirstPage; } }


        ///<summary>FirstPageNumber is the first page number in the set of Page Links</summary>
        public int FirstPageNumber
        {
            get
            {
                return CurrentState.CurrentPage;
            }
        }
        ///<summary>LastPageNumber is the last page number in the set of Page Links</summary>
        public int LastPageNumber
        {
            get
            {
                int last;
                int calulatedLast = FirstPageNumber + CurrentState.PageSize - 1;
                if (calulatedLast > LastPage)
                    last = LastPage;
                else
                    last = calulatedLast;
                return last;
            }
        }

        ///<summary> FromItems is the starting item number of your collection being shown
        public int FromItems
        {
            get
            {
                return (CurrentPage.Page - 1) * CurrentState.PageSize + 1;
            }
        }

        ///<summary> ToItems is the ending item number of your collection being shown
        public int ToItems
        {
            get
            {
                int calculatedToItems = FromItems + CurrentState.PageSize - 1;
                if (calculatedToItems > TotalItemCount)
                {
                    calculatedToItems = TotalItemCount;
                }
                return calculatedToItems;
            }
        }
        #endregion

        #region Constructor
        public Paginator(int totalItemCount, PageState currentState)
        {
            // 1) Set key data members
            TotalItemCount = totalItemCount;
            CurrentState = currentState; //PageRef.Page current page#, PageRef.Text page display text
            CurrentPage = new(CurrentState.CurrentPage, currentState.CurrentPage.ToString());
            // 2) Generate the list of page references
            //   <First> <Previous> 1,2,3,4,5,6,7,8,9,10 <Next> <Last> 
            PageReferences = new List<PageRef>();
            PageReferences.Add(new(FirstPage, FirstPageText));       //calculated property
            PageReferences.Add(new(PreviousPage, PreviousPageText)); //calculated property

            //the calculated pages
            for (int pageNumber = FirstPageNumber; pageNumber <= LastPageNumber; pageNumber++)
            {
                PageReferences.Add(new(pageNumber, pageNumber.ToString()));
            }

            PageReferences.Add(new(NextPage, NextPageText));         //calculated property
            PageReferences.Add(new(LastPage, LastPageText));         //calculated property
        }

        #endregion
        #region Methods Implementing IEnumerable<PageRef>
        public IEnumerator<PageRef> GetEnumerator()
        {
            return PageReferences.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return PageReferences.GetEnumerator();
        }
        #endregion
    }
}