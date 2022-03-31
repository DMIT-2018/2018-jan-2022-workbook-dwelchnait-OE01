#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Entities;
using ChinookSystem.ViewModels;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTrackServices
    {
        #region Constructor and Context Dependency
        private readonly ChinookContext _context;

        //obtan the context link from IServiceCollection when this
        //  set of services is injected into the "outside user"
        internal PlaylistTrackServices(ChinookContext context)
        {
            _context = context;
        }
        #endregion

        #region Queries
        public List<PlaylistTrackInfo> PlaylistTrack_Fetch_Playlist(string playlistname,
                                                                            string username)
        {
            IEnumerable<PlaylistTrackInfo> info = _context.PlaylistTracks
                                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                                       && x.Playlist.UserName.Equals(username))
                                                .Select(x => new PlaylistTrackInfo
                                                {
                                                    TrackId = x.TrackId,
                                                    TrackNumber = x.TrackNumber,
                                                    SongName = x.Track.Name,
                                                    Milliseconds = x.Track.Milliseconds
                                                })
                                                .OrderBy(x => x.TrackNumber);
            return info.ToList();
        }
        #endregion

        #region Commands
        public void PlaylistTrack_AddTrack(string playlistname, string username, int trackid)
        {
            //create local variables
            Track trackExists = null;
            PlaylistTrack playlisttrackExists = null;
            Playlist playlistExists = null;
            int tracknumber = 0;

            //create a List<Exception> to contain all discoverd errors
            List<Exception> errorlist = new List<Exception>();

            //Business logic
            //these are processing rules that need to be satisfied for valid data
            //  rule: a track can only exist once on the playlist
            //  rule: each track on a playlist is assigned a continuous track number
            //
            //if the business rules are passed, consider the data valid, then
            //  a) stage your transaction work (Adds, Updates, Deletes)
            //  b) execute a SINLGE .SaveChanges() - commits to database

            //parameter validation
            if (string.IsNullOrWhiteSpace(playlistname))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }

            trackExists = _context.Tracks
                        .Where(x => x.TrackId == trackid)
                        .FirstOrDefault();
            if (trackExists == null)
            {
                errorlist.Add(new Exception("Select track no longer is on file. Refresh track table."));
            }

            //business process
            playlistExists = _context.Playlists
                            .Where(x => x.Name.Equals(playlistname)
                                    && x.UserName.Equals(username))
                            .FirstOrDefault();
            if (playlistExists == null)
            {
                //new playlist
                playlistExists = new Playlist()
                {
                    Name = playlistname,
                    UserName = username
                };
                //stage (only in memory)
                _context.Playlists.Add(playlistExists);
                tracknumber = 1;
            }
            else
            {
                //existing playlist
                //rule: unique tracks on playlist tracks

                //1/2 of compound pkey in PlaylistTracks (PlaylistID)
                //  x.Playlist.Name.Equals(playlistname, StringComparison.OrdinalIgnoreCase)
                //  && x.Playlist.UserName.Equals(username)

                playlisttrackExists = _context.PlaylistTracks
                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username)
                                        && x.TrackId == trackid)
                                .FirstOrDefault();
                if(playlisttrackExists != null)
                {
                    var songname = _context.Tracks
                                .Where(x => x.TrackId == trackid)
                                .Select(x => x.Name)
                                .SingleOrDefault();
                    errorlist.Add(new Exception($"Selected track ({songname}) is already on the playlist."));
                }
                else
                {
                    //generate the next tracknumber
                    tracknumber = _context.PlaylistTracks
                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username))
                                .Count();
                    tracknumber++;
                }

            }
            //add the tracl to the playlist tracks
            playlisttrackExists = new PlaylistTrack();

            //load values
            playlisttrackExists.TrackId = trackid;
            playlisttrackExists.TrackNumber = tracknumber;

            //?? what about the second part of the primary key: PlaylistID on PlaylistTracks
            //if the playlist exists then we know the id: playlistExists.PlaylistID
            //BUT if the playlist is NEW, we DO NOT know the id

            //in the situation of a NEW playlist, even though we have
            //  created the playlist instance (see above) it is ONLY
            //  staged!!!
            //this means that the actual sql record has NOT yet been created
            //this means that the IDENTITY value for the new playlist DOES NOT
            //  yet exist. The value on the playlist instance (playlistExist)
            //  is zero (0).
            //THERFORE we have a serious problem

            //Solution
            //The solution is built into EntityFramework software and is based on using the
            //  navigational property in Playlist pointing to it's "child"

            //staging a typical Add in the past was to reference the entity
            //  and use the entity.Add(xxxx)
            //      _context.PlaylistTracks.Add(playlisttrackExist)
            //IF you use this statement the playlistid woulld be zero (0)
            //  causing your transaction to ABORT (foreign key error)
            //WHY? pkeys cannot be zero (0) 

            //INSTEAD: do the staging using the "parentinstance.navchildproperty.Add(xxxx)
            playlistExists.PlaylistTracks.Add(playlisttrackExists);

            //Staging is complete
            //Commit the work (Transaction)
            //commiting the work needs a .SaveChanges() (send to database)
            //a transaction has ONLY ONE .Savechanges()
            //BUT what if you have discovered error(s) during the business process??
            //  if so, the throw all the errors and DO NOT COMMIT!!!!
            if (errorlist.Count > 0)
            {
                throw new AggregateException("Unable to add new track. Check concerns.", errorlist);
            }
            else
            {
                //consider data valid
                //has passed business processing rules
                _context.SaveChanges(); //commit to the database
            }

        }
        public void PlaylistTrack_RemoveTracks(string playlistname, string username, 
                    List<PlaylistTrackMove> trackstoremove)
        {
            Track trackExists = null;
            PlaylistTrack playlisttrackExists = null;
            Playlist playlistExists = null;
            int tracknumber = 0;
            List<Exception> errorlist = new List<Exception>();
            if (string.IsNullOrWhiteSpace(playlistname))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            if (trackstoremove.Count == 0)
            {
                throw new ArgumentNullException("No track list has been supplied");
            }
            playlistExists = _context.Playlists
                               .Where(x => x.Name.Equals(playlistname)
                                       && x.UserName.Equals(username))
                               .FirstOrDefault();
            if (playlistExists == null)
            {
                errorlist.Add(new Exception("Playlist does not exist. Refresh playlist search"));
            }
            else
            {
                IEnumerable<PlaylistTrackMove> removelist = trackstoremove
                                                        .Where(x => x.SelectedTrack);
                IEnumerable<PlaylistTrackMove> keeplist = trackstoremove
                                                        .Where(x => !x.SelectedTrack)
                                                        .OrderBy(x => x.TrackNumber);
                foreach(var track in removelist)
                {
                    playlisttrackExists =  _context.PlaylistTracks
                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username)
                                        && x.TrackId == track.TrackId)
                                .FirstOrDefault();
                    if (playlisttrackExists != null)
                    {
                        _context.PlaylistTracks.Remove(playlisttrackExists);
                    }
                    //if the track does not exist, then there is actually no problem
                    //Why? because your were going to remove it anyways
                }

                tracknumber = 1;
                foreach(var track in keeplist)
                {
                    playlisttrackExists =  _context.PlaylistTracks
                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username)
                                        && x.TrackId == track.TrackId)
                                .FirstOrDefault();
                    if (playlisttrackExists != null)
                    {
                        playlisttrackExists.TrackNumber = tracknumber;
                        EntityEntry<PlaylistTrack> updating = _context.Entry(playlisttrackExists);
                        updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        tracknumber++;
                    }
                    else
                    {
                        var songname = _context.Tracks
                               .Where(x => x.TrackId == track.TrackId)
                               .Select(x => x.Name)
                               .SingleOrDefault();
                        errorlist.Add(new Exception($"Track {songname} does not exist. Refresh playlist search"));
                    }
                }
            }
            //end of transaction
            if (errorlist.Count > 0)
            {
                throw new AggregateException("Unable to remove tracks. Check concerns.", errorlist);
            }
            else
            {
                _context.SaveChanges();
            }
        }

        public void PlaylistTrack_MoveTracks(string playlistname, string username,
                    List<PlaylistTrackMove> trackstomove)
        {
            Track trackExists = null;
            PlaylistTrack playlisttrackExists = null;
            Playlist playlistExists = null;
            int tracknumber = 0;
            List<Exception> errorlist = new List<Exception>();
            if (string.IsNullOrWhiteSpace(playlistname))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException("Playlist name is missing");
            }
            if (trackstomove.Count == 0)
            {
                throw new ArgumentNullException("No track list has been supplied");
            }
            playlistExists = _context.Playlists
                               .Where(x => x.Name.Equals(playlistname)
                                       && x.UserName.Equals(username))
                               .FirstOrDefault();
            if (playlistExists == null)
            {
                errorlist.Add(new Exception("Playlist does not exist. Refresh playlist search"));
            }
            else
            {
                //use the List<T>.Sort()
                trackstomove.Sort((x, y) => x.TrackInput.CompareTo(y.TrackInput));

                //validation
                // a) numeric and positive non-zero
                int tempNum = 0;
                foreach(var track in trackstomove)
                {
                    var songname = _context.Tracks
                               .Where(x => x.TrackId == track.TrackId)
                               .Select(x => x.Name)
                               .SingleOrDefault();

                    if (int.TryParse(track.TrackInput, out tempNum))
                    {
                        if (tempNum < 1)
                        {
                            errorlist.Add(new Exception($"{songname}'s re-organize value needs to be greater than 0. Example: 3 "));
                        }
                    }
                    else
                    {
                        errorlist.Add(new Exception($"{songname}'s re-organize value needs to be a number. Example: 3 "));
                    }
                }
                // b) unique re-sequence track number
                for(int i = 0; i < trackstomove.Count - 1; i++)
                {
                    var songname1 = _context.Tracks
                           .Where(x => x.TrackId == trackstomove[i].TrackId)
                           .Select(x => x.Name)
                           .SingleOrDefault();
                    var songname2 = _context.Tracks
                           .Where(x => x.TrackId == trackstomove[i + 1].TrackId)
                           .Select(x => x.Name)
                           .SingleOrDefault();
                    if (trackstomove[i] == trackstomove[i+1])
                    {
                        errorlist.Add(new Exception($"{songname1}'s re-organize value in the same as {songname2}. Re-organization values must be different. "));
                    }
                }
                //re=sequence the playlist
                tracknumber = 1;
                foreach (var track in trackstomove)
                {
                    playlisttrackExists =  _context.PlaylistTracks
                                .Where(x => x.Playlist.Name.Equals(playlistname)
                                        && x.Playlist.UserName.Equals(username)
                                        && x.TrackId == track.TrackId)
                                .FirstOrDefault();
                    if (playlisttrackExists != null)
                    {
                        playlisttrackExists.TrackNumber = tracknumber;
                        EntityEntry<PlaylistTrack> updating = _context.Entry(playlisttrackExists);
                        updating.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        tracknumber++;
                    }
                    else
                    {
                        var songname = _context.Tracks
                               .Where(x => x.TrackId == track.TrackId)
                               .Select(x => x.Name)
                               .SingleOrDefault();
                        errorlist.Add(new Exception($"{songname} does not exist on your playlist. Refresh playlist search"));
                    }
                }
            }
            //end of transaction
            if (errorlist.Count > 0)
            {
                throw new AggregateException("Unable to remove tracks. Check concerns.", errorlist);
            }
            else
            {
                _context.SaveChanges();
            }
        }

        #endregion
    }
}
