namespace CrabSeek.Models
{
    /// <summary>
    /// The result of a pathseeking request.
    /// </summary>
    public class PathSeekerResponse
    {
        /// <summary>
        /// The node path, from start to finish, if found.
        /// </summary>
        public IEnumerable<XY>? Path { get; set; }

        /// <summary>
        /// The result of a pathseeking request.
        /// </summary>
        public PathSeekerResult Result { get; set; } = PathSeekerResult.NotSearched;
    }
}
