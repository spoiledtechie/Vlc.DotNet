using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;

namespace Vlc.DotNet.Wpf.Window
{
    /// <summary>
    /// this control is built upon wpf content control and no longer has airspace issues.
    /// due to the VLCLib library, it still requires a Handle to attach it self to
    /// so this control will take up the full window, but no longer has airspace issues 
    /// as you can now draw things over it
    /// </summary>
    public partial class VlcControl : ContentControl, ISupportInitialize
    {
        private VlcMediaPlayer myVlcMediaPlayer;

        public string VlcLibDirectory { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public bool IsPlaying
        {
            get { return myVlcMediaPlayer.IsPlaying(); }
        }

        public void BeginInit()
        {
        }

        private IntPtr _handle;

        public void SetHandle(IntPtr handle)
        {
            _handle = handle;
            myVlcMediaPlayer.VideoHostControlHandle = handle;
        }

        public void EndInit()
        {
            if (IsInDesignMode() || myVlcMediaPlayer != null)
                return;
            DirectoryInfo dir = null;
            if (VlcLibDirectory == null && (dir = OnVlcLibDirectoryNeeded()) == null)
            {
                throw new Exception("'VlcLibDirectory' must be set.");
            }
            myVlcMediaPlayer = new VlcMediaPlayer(new DirectoryInfo(VlcLibDirectory));
            myVlcMediaPlayer.VideoHostControlHandle = _handle;
            RegisterEvents();

        }

        // work around http://stackoverflow.com/questions/34664/designmode-with-controls/708594
        private static bool IsInDesignMode()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location.Contains("VisualStudio");
        }

        public event EventHandler<VlcLibDirectoryNeededEventArgs> VlcLibDirectoryNeeded;

        protected void Dispose(bool disposing)
        {
            if (myVlcMediaPlayer != null)
            {
                UnregisterEvents();
                if (IsPlaying)
                    Stop();
                myVlcMediaPlayer.Dispose();
                //base.Dispose(disposing);
                GC.SuppressFinalize(this);
            }
        }

        public DirectoryInfo OnVlcLibDirectoryNeeded()
        {
            var del = VlcLibDirectoryNeeded;
            if (del != null)
            {
                var args = new VlcLibDirectoryNeededEventArgs();
                del(this, args);
                return args.VlcLibDirectory;
            }
            return null;
        }

        public void Play()
        {
            EndInit();
            myVlcMediaPlayer.Play();
        }

        public Rectangle GetRectangle()
        {
            return myVlcMediaPlayer.GetRectangle();
        }

        public void Play(FileInfo file, params string[] options)
        {
            EndInit();
            myVlcMediaPlayer.SetMedia(file, options);
            Play();
        }

        public void Play(Uri uri, params string[] options)
        {
            EndInit();
            myVlcMediaPlayer.SetMedia(uri, options);
            Play();
        }

        public void Pause()
        {
            EndInit();
            myVlcMediaPlayer.Pause();
        }

        public void Stop()
        {
            EndInit();
            myVlcMediaPlayer.Stop();
        }

        public VlcMedia GetCurrentMedia()
        {
            EndInit();
            return myVlcMediaPlayer.GetMedia();
        }

        public float Position
        {
            get { return myVlcMediaPlayer.Position; }
            set { myVlcMediaPlayer.Position = value; }
        }

        public IChapterManagement Chapter
        {
            get
            {
                return myVlcMediaPlayer.Chapters;
            }
        }

        public float Rate
        {
            get { return myVlcMediaPlayer.Rate; }
            set { myVlcMediaPlayer.Rate = value; }
        }


        public int Volume
        {
            get { return myVlcMediaPlayer.Volume; }
            set
            {
                if (myVlcMediaPlayer != null)
                    myVlcMediaPlayer.Volume = value;
            }
        }

        public MediaStates State
        {
            get { return myVlcMediaPlayer.State; }
        }

        public ISubTitlesManagement SubTitles
        {
            get
            {
                return myVlcMediaPlayer.SubTitles;
            }
        }

        public IVideoManagement Video
        {
            get
            {
                return myVlcMediaPlayer.Video;
            }
        }

        public IAudioManagement Audio
        {
            get
            {
                return myVlcMediaPlayer.Audio;
            }
        }

        public long Length
        {
            get { return myVlcMediaPlayer.Length; }
        }

        public long Time
        {
            get { return myVlcMediaPlayer.Time; }
            set { myVlcMediaPlayer.Time = value; }
        }

        public void SetMedia(FileInfo file, params string[] options)
        {
            EndInit();
            myVlcMediaPlayer.SetMedia(file, options);
        }

        public void SetMedia(Uri file, params string[] options)
        {
            EndInit();
            myVlcMediaPlayer.SetMedia(file, options);
        }
    }
}