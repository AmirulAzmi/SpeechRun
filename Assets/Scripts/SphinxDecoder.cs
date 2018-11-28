using System;
using System.Runtime.InteropServices;

namespace Pocketsphinx {
	
	public class SphinxDecoder {

		internal readonly IntPtr _ps;

		private int _bufferOffset = 0;

		private readonly Int16[] _buffer;

		private LogMath _logMath;

		[DllImport("pocketsphinx")]
		private static extern IntPtr ps_init(IntPtr config);

		public SphinxDecoder(SphinxConfig config, uint bufferSize = 4096) {
			_ps = ps_init (config._cmdln);

			if (IntPtr.Zero.Equals (_ps)) {
				throw new Exception ("ps_init failed");
			}

			_buffer = new Int16[bufferSize];
		}

		[DllImport("pocketsphinx")]
		private static extern void ps_free (IntPtr ps);

		~SphinxDecoder() {
			ps_free (_ps);
		}

		[DllImport("pocketsphinx")]
		private static extern Int32 ps_get_in_speech(IntPtr ps);

		public bool InSpeech {
			get { 
				return ps_get_in_speech (_ps) != 0;
			}
		}

		[DllImport("pocketsphinx")]
		private static extern Int32 ps_process_raw(
			IntPtr ps,
			Int16[] buffer,
			UInt32 nSamples,
			Int32 no_search,
			Int32 full_utt);

		public void ProcessRaw(float[] source, int offset, int length) {
			int available = _buffer.Length - _bufferOffset;

			if (length < available) {
				CopyToBuffer (source, offset, length);
				_bufferOffset += length;
			} 

			else {
				while (length >= available) {
					CopyToBuffer (source, offset, available);
					ps_process_raw (_ps, _buffer, (uint)_buffer.Length, 0, 0);

					offset += available;
					length -= available;
					_bufferOffset = 0;
					available = _buffer.Length;
				}

				CopyToBuffer (source, offset, length);
				_bufferOffset += length;
			}
		}

		[DllImport("pocketsphinx")]
		private static extern IntPtr ps_get_logmath (IntPtr ps);

		public LogMath LogMath {
			get { 
				if(_logMath == null) _logMath = new LogMath(ps_get_logmath(_ps));
					return _logMath;
			}
		}

		[DllImport("pocketsphinx")]
		private static extern Int32 ps_start_utt (
			IntPtr ps, 
			[MarshalAs (UnmanagedType.LPStr)] string uttid);

		public void StartUtterance(string uttid = null) {
			ps_start_utt(_ps, uttid);
			_bufferOffset = 0;
		}

		void CopyToBuffer(float[] source, int offset, int length) {
			float f;
			for (int i = 0; i < length; i++) {
				f = source [i + offset];

				if (f < 0f) {
					_buffer [i + _bufferOffset] = (Int16)(-f * Int16.MinValue);
				} 

				else {
					_buffer [i + _bufferOffset] = (Int16)(f * Int16.MaxValue);
				}
			}
		}

		[DllImport("pocketsphinx")]
		private static extern Int32 ps_end_utt (IntPtr ps);

		public void EndUtterance() {
			if (_bufferOffset > 0) {
				ps_process_raw (_ps, _buffer, (uint)_bufferOffset, 0, 0);
			}

			ps_end_utt (_ps);
		}

		[DllImport("pocketsphinx")]
		private static extern IntPtr ps_get_hyp (
			IntPtr ps,
			out Int32 bestScore,
			[MarshalAs (UnmanagedType.LPStr)] out string uttid);

		public string GetHypothesis(out float score, out string uttid) {
			Int32 logScore;
			IntPtr hyp = ps_get_hyp (_ps, out logScore, out uttid);
			score = (float)LogMath.Exp (logScore);

			return System.Runtime.InteropServices.Marshal.PtrToStringAnsi (hyp);
		}
	}
}