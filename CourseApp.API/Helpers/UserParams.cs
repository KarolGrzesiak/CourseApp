namespace CourseApp.API.Helpers
{
    public class UserParams
    {
        private int _maxPageSize = Constants.MaxPageSize;
        private int _pageSize = Constants.PageSize;

        public int PageNumber { get; set; } = 1;


        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > _maxPageSize) ? _maxPageSize : value; }
        }

        public int UserId { get; set; }
        public string Role { get; set; } = Constants.StudentRole;
        public int MinAge { get; set; } = Constants.MinAge;
        public int MaxAge { get; set; } = Constants.MaxAge;
        public string OrderBy { get; set; }


    }
}