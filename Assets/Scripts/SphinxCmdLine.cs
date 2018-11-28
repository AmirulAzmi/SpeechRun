using System;
using System.Runtime.InteropServices;

namespace Pocketsphinx {
	
	public class SphinxConfig {
			
		internal readonly IntPtr _cmdln;

		[DllImport("pocketsphinx")]
		private static extern IntPtr ps_args();

		[DllImport("sphinxbase")]
		private static extern IntPtr cmd_ln_init(
			IntPtr inout_cmdln,
			IntPtr defn,
			Int32 strict,
			IntPtr _nl);

		public SphinxConfig() {
			_cmdln = cmd_ln_init (IntPtr.Zero, ps_args(), 1, IntPtr.Zero);

			if (IntPtr.Zero.Equals (_cmdln)) {
				throw new Exception ("cmd_ln_init returned null");
			}
		}

		[DllImport("sphinxbase")]
		private static extern void cmd_ln_set_str_r(
			IntPtr cmdln,
			[MarshalAs(UnmanagedType.LPStr)] string name,
			[MarshalAs(UnmanagedType.LPStr)] string str);

		public string Logfn {
			set {
				cmd_ln_set_str_r (_cmdln, "-logfn", value);
			}
		}

		public string Hmm {
			set {
				cmd_ln_set_str_r (_cmdln, "-hmm", value);
			}
		}

		public string Lm {
			set { 
				cmd_ln_set_str_r (_cmdln, "-lm", value);
			}
		}

		public string Kws {
			set { 
				cmd_ln_set_str_r (_cmdln, "-kws", value);
			}
		}

		public string Jsgf {
			set { 
				cmd_ln_set_str_r (_cmdln, "-jsgf", value);
			}
		}

		public string Dict {
			set { 
				cmd_ln_set_str_r (_cmdln, "-dict", value);
			}
		}
	}
}
