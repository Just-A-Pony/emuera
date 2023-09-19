using System;
using System.IO;
using NAudio.Wave;
using NAudio.Vorbis;
using NAudio.Extras;
using MinorShift.Emuera.GameProc;
using MinorShift.Emuera.GameView;

namespace MinorShift._Library
{
	internal class Sound
	{
		private string _filename;
		private WaveOutEvent player;
		private WaveStream source;
		private float _volume = 1.0f;
		private bool is_loop_stream = false;
		public void play(string filename, bool loop = false)
		{
			if (source != null && (_filename != filename || is_loop_stream != loop)) {
				source.Dispose();
				source = null;
			}
			_filename = filename;
			if (source == null) {
				if (_filename.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase)) {
					source = new VorbisWaveReader(_filename);
				}
				else {
					source = new AudioFileReader(_filename);
				}
				if (loop) {
					source = new LoopStream(source);
				}
				is_loop_stream = loop;
				if (player != null) {
					player.Dispose();
				}
				player = new WaveOutEvent();
				player.Init(source);
				player.Volume = _volume;
			}
			else {
				player.Stop();
				if (source.CanSeek) {
					source.Seek(0, SeekOrigin.Begin);
				}
			}
			player.Play();
		}

		public void stop()
		{
			if (player != null) {
				player.Stop();
				if (source != null && source.CanSeek) {
					source.Seek(0, SeekOrigin.Begin);
				}
			}
		}

		public void close()
		{
			if (player != null) {
				player.Stop();
				player.Dispose();
				player = null;
			}
			if (source != null) {
				source.Dispose();
				source = null;
			}
		}

		public bool isPlaying() {
			return (player != null && player.PlaybackState == PlaybackState.Playing);
		}

		public void setVolume(int volume)
		{
			_volume = volume / 100.0f;
			if (player != null) {
				player.Volume = _volume;
			}
		}
	}
}
