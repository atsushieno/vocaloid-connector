using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Commons.VocaloidApi
{
	public class SocketVocaloidConnector : IVocaloidApi
	{
		#region internals

		public SocketVocaloidConnector (string host, int port)
		{
			this.host = host;
			this.port = port;
			processor = new JsonRequestProcessor (ProcessRequest);
		}

		public void Start ()
		{
			tcp = new TcpClient (host, port);
		}

		public void Quit ()
		{
			tcp.Close ();
		}

		TcpClient tcp;
		string host;
		int port;
		JsonRequestProcessor processor;

		string ProcessRequest (string request)
		{
			var bytes = Encoding.UTF8.GetBytes (request);
			var s = tcp.GetStream ();
			s.Write (bytes, 0, bytes.Length);
			var ret = "";
			do {
				int size = s.Read (bytes, 0, bytes.Length);
				var str = Encoding.UTF8.GetString (bytes, 0, size);
				int idx = str.IndexOf ('\0');
				if (idx >= 0)
					return ret + str.Substring (0, idx);
				else
					ret += str;
			} while (true);
		}

		void ProcessEmpty (string functionName, params object [] args)
		{
			processor.ProcessEmpty (functionName, args);
		}

		IDictionary<string,object> ProcessUntyped (string functionName, params object [] args)
		{
			return processor.ProcessUntyped (functionName, args);
		}
		
		T Process<T> (string functionName, params object [] args)
		{
			return processor.Process<T> (functionName, args);
		}

		Result<T> ProcessWithResult<T> (string functionName, params object [] args)
		{
			return processor.ProcessWithResult<T> (functionName, args);
		}

		#endregion

		#region Dialog API

		// VSBool VSDlgAddField(VSFlexDlgField field)
		public bool DialogAddField (DialogField field)
		{
			return Process<bool> ("VSDlgAddField", field);
		}
		
		// void VSDlgSetDialogTitle(VSCString dlgTitle)
		public void DialogSetDialogTitle (string dialogTitle)
		{
			ProcessEmpty ("VSDlgSetDialogTitle", dialogTitle);
		}
		
		// VSInt32 VSDlgDoModal()
		public int DialogDoModal ()
		{
			return Process<int> ("VSDlgDoModal");
		}
		
		// VSBool result, VSInt32 value VSDlgGetIntValue(VSCString fieldName)
		public Result<int> DialogGetIntValue (string fieldName)
		{
			return ProcessWithResult<int> ("VSDlgGetIntValue", fieldName);
		}
		
		// VSBool result, VSBool value VSDlgGetBoolValue(VSCString fieldName)
		public Result<bool> DialogGetBoolValue (string fieldName)
		{
			return ProcessWithResult<bool> ("VSDlgGetBoolValue", fieldName);
		}

		// VSBool result, VSFloat value VSDlgGetFloatValue(VSCString fieldName)
		public Result<float> DialogGetFloatValue (string fieldName)
		{
			return ProcessWithResult<float> ("VSDlgGetFloatValue", fieldName);
		}

		// VSBool result, VSString value VSDlgGetStringValue(VSCString fieldName)
		public Result<string> DialogGetStringValue (string fieldName)
		{
			return ProcessWithResult<string> ("VSDlgGetStringValue", fieldName);
		}

		#endregion

		#region Note Event API

		// void VSSeekToBeginNote()
		public void SeekToBeginNote ()
		{
			ProcessEmpty ("VSSeekToBeginNote");
		}

		// VSBool result, VSLuaNote note VSGetNextNote()
		public Result<Note> GetNextNote ()
		{
			return ProcessWithResult<Note> ("VSGetNextNote");
		}

		// VSBool VSUpdateNote(VSLuaNote note)
		public bool UpdateNote (Note note)
		{
			return Process<bool> ("VSUpdateNote", note);
		}

		// VSBool VSInsertNote(VSLuaNote note)
		public bool InsertNote (Note note)
		{
			return Process<bool> ("VSInsertNote", note);
		}

		// VSBool VSRemoveNote(VSLuaNote note)
		public bool RemoveNote (Note note)
		{
			return Process<bool> ("VSRemoveNote", note);
		}
		
		// VSBool result, VSLuaNoteEx note VSGetNextNoteEx()
		public Result<NoteEx> GetNextNoteEx ()
		{
			return ProcessWithResult<NoteEx> ("VSGetNextNoteEx");
		}

	
		// VSBool VSUpdateNoteEx(VSLuaNoteEx note)
		public bool UpdateNoteEx (NoteEx note)
		{
			return Process<bool> ("VSUpdateNoteEx", note);
		}

		// VSBool VSInsertNoteEx(VSLuaNoteEx note)
		public bool InsertNoteEx (NoteEx note)
		{
			return Process<bool> ("VSInsertNoteEx", note);
		}

		#endregion

		#region Control Parameter API

		// VSBool result, VSInt32 value VSGetControlAt(VSCString controlType, VSInt32 posTick)
		public Result<int> GetControlAt (string controlType, int positionTick)
		{
			return ProcessWithResult<int> ("VSGetControlAt", controlType, positionTick);
		}
		
		// VSBool VSUpdateControlAt(VSCString controlType, VSInt32 posTick, VSInt32 value)
		public bool UpdateControlAt (string controlType, int positionTick, int value)
		{
			return Process<bool> ("VSUpdateControlAt", controlType, positionTick, value);
		}

		// VSBool VSSeekToBeginControl(VSCString controlType)
		public bool SeekToBeginControl (string controlType)
		{
			return Process<bool> ("VSSeekToBeginControl", controlType);
		}
		
		// VSBool result, VSLuaControl control VSGetNextControl(VSCString controlType)
		public Result<Control> GetNextControl (string controlType)
		{
			return ProcessWithResult<Control> ("VSGetNextControl", controlType);
		}

		// VSBool VSUpdateControl(VSLuaControl control)
		public bool UpdateControl (Control control)
		{
			return Process<bool> ("VSUpdateControl", control);
		}

		// VSBool VSInsertControl(VSLuaControl control)
		public bool InsertControl (Control control)
		{
			return Process<bool> ("VSInsertControl", control);
		}
		
		// VSBool VSRemoveControl(VSLuaControl control)
		public bool RemoveControl (Control control)
		{
			return Process<bool> ("VSRemoveControl", control);
		}
		
		// VSBool result, VSInt32 value VSGetDefaultControlValue(VSCString controlType)
		public Result<int> GetDefaultControlValue (string controlType)
		{
			return ProcessWithResult<int> ("VSGetDefaultControlValue", controlType);
		}
		
		#endregion

		#region Master Track API
		
		// void VSSeekToBeginTempo()
		public void SeekToBeginTempo ()
		{
			ProcessEmpty ("VSSeekToBeginTempo");
		}
		
		// void VSSeekToBeginTimeSig()
		public void SeekToBeginTimeSig ()
		{
			ProcessEmpty ("VSSeekToBeginTimeMsg");
		}
		
		// VSBool result, VSLuaTempo tempo VSGetNextTempo()
		public Result<Tempo> GetNextTempo ()
		{
			return ProcessWithResult<Tempo> ("VSGetNextTempo");
		}
		
		// VSBool result, VSLuaTimeSig timeSig VSGetNextTimeSig()
		public Result<TimeSignature> GetNextTimeSignature ()
		{
			return ProcessWithResult<TimeSignature> ("VSGetNextTimeSig");
		}
		
		// VSBool result, VSFloat tempo VSGetTempoAt(VSInt32 posTick)
		public Result<float> GetTempoAt (int positionTick)
		{
			return ProcessWithResult<float> ("VSGetTempoAt", positionTick);
		}
		
		// VSBool result, VSInt32 numerator, VSInt32 denominator VSGetTimeSigAt(VSInt32 posTick)
		// we need non-void template parameter, so we return tick-pointless TimeSignature here.
		public Result<TimeSignature> GetTimeSignatureAt (int positionTick)
		{
			var ret = ProcessUntyped ("VSGetTimeSigAt", positionTick);
			var ts = new TimeSignature (0, (int) ret ["numerator"], (int) ret ["denominator"]);
			return new Result<TimeSignature> ((bool) ret ["result"], ts);
		}
		
		// VSCString VSGetSequenceName()
		public string GetSequenceName ()
		{
			return Process<string> ("VSGetSequenceName");
		}
		
		// VSCString VSGetSequencePath()
		public string GetSequencePath ()
		{
			return Process<string> ("VSGetSequencePath");
		}
		
		// VSInt32 VSGetResolution ()
		public int GetResolution ()
		{
			return Process<int> ("VSGetResolution");
		}
		
		#endregion

		#region Musical Part API

		// VSBool result, VSLuaMusicalPart part VSGetMusicalPart()
		public Result<MusicalPart> GetMusicalPart ()
		{
			return ProcessWithResult<MusicalPart> ("VSGetMusicalPart");
		}

		// VSBool VSUpdateMusicalPart(VSLuaMusicalPart part)
		public bool UpdateMusicalPart (MusicalPart part)
		{
			return Process<bool> ("VSUpdateMusicalPart", part);
		}

		// VSBool result, VSLuaMusicalSinger VSGetMusicalPartSinger()
		public Result<MusicalSinger> GetMusicalPartSinger ()
		{
			return ProcessWithResult<MusicalSinger> ("VSGetMusicalPartSinger");
		}

		#endregion

		#region WAV Part API
		
		// VSBool result, VSLuaWAVPart wavPart VSGetStereoWAVPart()
		public Result<WavPart> GetStereoWavPart ()
		{
			return ProcessWithResult<WavPart> ("VSGetStereoWAVPart");
		}
		
		// void VSSeekToBeginMonoWAVPart()
		public void SeekToBeginMonoWavPart ()
		{
			ProcessEmpty ("VSSeekToBeginMonoWAVPart");
		}
		
		// VSBool result, VSLuaWAVPart wavPart VSGetNextMonoWAVPart()
		public Result<WavPart> GetNextMonoWavPart ()
		{
			return ProcessWithResult<WavPart> ("VSGetNextMonoWAVPart");
		}
		
		// VSInt32 VSMessageBox(VSCString message, VSUInt32 type)
		public MessageBoxResult MessageBox (string message, MessageBoxStyle type)
		{
			return Process<MessageBoxResult> ("VSGetNextMonoWAVPart", message, type);
		}
		
		#endregion
	}
}
