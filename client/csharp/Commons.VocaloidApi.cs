using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Commons.VocaloidApi
{
	public class RegisterAttribute : Attribute
	{
		public RegisterAttribute (string name)
		{
			Name = name;
		}
		
		public string Name { get; private set; }
	}
	
	// used for methods that returns boolean result and whatever.
	public struct Result<T>
	{
		public Result (bool success, T value)
		{
			Success = success;
			Value = value;
		}
		
		public bool Success;
		public T Value;
	}

	[Register ("VSLuaNote")]
	public class Note
	{
		[Register ("posTick")]
		public int PositionTick;
		
		[Register ("durTick")]
		public int DurationTick;
		
		[Register ("noteNum")]
		public int NoteNumber;
		
		[Register ("velocity")]
		public int Velocity;
		
		[Register ("lyric")]
		public string Lyric;
		
		[Register ("phenomes")]
		public string Phonemes;
	}
	
	[Register ("VSLuaNoteEx")]
	public class NoteEx
	{
		[Register ("posTick")]
		public int PositionTick;
		
		[Register ("durTick")]
		public int DurationTick;
		
		[Register ("noteNum")]
		public int NoteNumber;
		
		[Register ("velocity")]
		public int Velocity;
		
		[Register ("lyric")]
		public string Lyric;
		
		[Register ("phenomes")]
		public string Phonemes;

		[Register ("bendDepth")]
		public int BendDepth;
		[Register ("bendLength")]
		public int BendLength;
		[Register ("risePort")]
		public bool RisePortamento;
		[Register ("fallPort")]
		public bool FallPortamento;
		[Register ("decay")]
		public int Decay;
		[Register ("accent")]
		public int Accent;
		[Register ("opening")]
		public int Opening;
		
		[Register ("vibratoLength")]
		public int VibratoLength;

		// 0: no vibrato, 1-4: normal, 5-8: extreme, 9-12: fast, 13-16: slight
		[Register ("vibratoType")]
		public int VibratoType;
	}

	[Register ("VSLuaControl")]
	public class Control
	{
		public const string DYN = "DYN";
		public const string BRE = "BRE";
		public const string BRI = "BRI";
		public const string CLE = "CLE";
		public const string GEN = "GEN";
		public const string PIT = "PIT";
		public const string PBS = "PBS";
		public const string POR = "POR";
		
		public const string Dynamics = "DYN";
		public const string Breathiness = "BRE";
		public const string Brightness = "BRI";
		public const string Clearness = "CLE";
		public const string Gender = "GEN";
		public const string PitchBend = "PIT";
		public const string PitchBendSensitivity = "PBS";
		public const string Portamento = "POR";
		
		[Register ("posTick")]
		public int PositionTick;
		
		[Register ("value")]
		public int Value;
		
		[Register ("type")]
		public string Type;
	}
	
	[Register ("VSLuaTempo")]
	public class Tempo
	{
		[Register ("posTick")]
		public int PositionTick;
		[Register ("value")]
		public float Value;
	}

	[Register ("VSLuaTimeSig")]
	public class TimeSignature
	{
		public TimeSignature (int positionTick, int numerator, int denominator)
		{
			PositionTick = positionTick;
			Numerator = numerator;
			Denominator = denominator;
		}
		
		[Register ("posTick")]
		public int PositionTick;
		
		[Register ("numerator")]
		public int Numerator;
		
		[Register ("denominator")]
		public int Denominator;
	}

	[Register ("VSLuaMusicalPart")]
	public class MusicalPart
	{
		[Register ("posTick")]
		public int PositionTick;
		[Register ("playTime")]
		public int PlayTime;
		[Register ("durTick")]
		public int DurationTick;
		[Register ("name")]
		public string Name;
		[Register ("comment")]
		public string Comment;
	}

	[Register ("VSLuaMusicalSinger")]
	public class MusicalSinger
	{
		[Register ("vBS")]
		public int BankSelect;
		[Register ("vPC")]
		public int ProgramChange;

		[Register ("breathiness")]
		public int Breathiness;
		[Register ("brightness")]
		public int Brightness;
		[Register ("clearness")]
		public int Clearness;
		[Register ("genderFactor")]
		public int Gender;
		[Register ("opening")]
		public int Opening;

		[Register ("compID")]
		public string ComponentId;
	}

	// VSLuaWAVPart
	[Register ("VSLuaWAVPart")]
	public class WavPart
	{
		[Register ("posTick")]
		public int PositionTick;
		[Register ("playTime")]
		public int PlayTime;
		[Register ("sampleRate")]
		public int SampleRate;
		[Register ("sampleReso")]
		public int SampleResolution;
		[Register ("channels")]
		public int Channels;
		[Register ("name")]
		public string Name;
		[Register ("comment")]
		public string Comment;
		[Register ("filePath")]
		public string FilePath;
	}
	
	[Register ("VSFlexDlgField")]
	public class DialogField
	{
		[Register ("name")]
		public string Name;
		[Register ("caption")]
		public string Caption;
		[Register ("initialVal")]
		public string InitialValue;
		[Register ("type")]
		public DialogFieldType Type;
	}

	// VSFlexDlgFieldType
	public enum DialogFieldType
	{
		Integer,
		Bool,
		Float,
		String,
		StringList
	}

	public enum MessageBoxStyle
	{
		OK,
		OKCancel,
		AbortRetryIgnore,
		YesNoCancel,
		YesNo,
		RetryCancel
	}

	public enum MessageBoxResult
	{
		OK = 1,
		Cancel,
		Abort,
		Retry,
		Ignore,
		Yes,
		No
	}

	public interface IVocaloidApi
	{
		#region Dialog API

		// VSBool VSDlgAddField(VSFlexDlgField field)
		bool DialogAddField (DialogField field);
		
		// void VSDlgSetDialogTitle(VSCString dlgTitle)
		void DialogSetDialogTitle (string dialogTitle);
		
		// VSInt32 VSDlgDoModal()
		int DialogDoModal ();
		
		// VSBool result, VSInt32 value VSDlgGetIntValue(VSCString fieldName)
		Result<int> DialogGetIntValue (string fieldName);
		
		// VSBool result, VSBool value VSDlgGetBoolValue(VSCString fieldName)
		Result<bool> DialogGetBoolValue (string fieldName);

		// VSBool result, VSFloat value VSDlgGetFloatValue(VSCString fieldName)
		Result<float> DialogGetFloatValue (string fieldName);

		// VSBool result, VSString value VSDlgGetStringValue(VSCString fieldName)
		Result<string> DialogGetStringValue (string fieldName);

		#endregion

		#region Note Event API

		// void VSSeekToBeginNote()
		void SeekToBeginNote ();

		// VSBool result, VSLuaNote note VSGetNextNote()
		Result<Note> GetNextNote ();

		// VSBool VSUpdateNote(VSLuaNote note)
		bool UpdateNote (Note note);

		// VSBool VSInsertNote(VSLuaNote note)
		bool InsertNote (Note note);

		// VSBool VSRemoveNote(VSLuaNote note)
		bool RemoveNote (Note note);

		// VSBool result, VSLuaNoteEx note VSGetNextNoteEx()
		Result<NoteEx> GetNextNoteEx ();

		// VSBool VSUpdateNoteEx(VSLuaNoteEx note)
		bool UpdateNoteEx (NoteEx note);

		// VSBool VSInsertNoteEx(VSLuaNoteEx note)
		bool InsertNoteEx (NoteEx note);

		#endregion

		#region Control Parameter API

		// VSBool result, VSInt32 value VSGetControlAt(VSCString controlType, VSInt32 posTick)
		Result<int> GetControlAt (string controlType, int positionTick);
		
		// VSBool VSUpdateControlAt(VSCString controlType, VSInt32 posTick, VSInt32 value)
		bool UpdateControlAt (string controlType, int positionTick, int value);

		// VSBool VSSeekToBeginControl(VSCString controlType)
		bool SeekToBeginControl (string controlType);
		
		// VSBool result, VSLuaControl control VSGetNextControl(VSCString controlType)
		Result<Control> GetNextControl (string controlType);

		// VSBool VSUpdateControl(VSLuaControl control)
		bool UpdateControl (Control control);

		// VSBool VSInsertControl(VSLuaControl control)
		bool InsertControl (Control control);
		
		// VSBool VSRemoveControl(VSLuaControl control)
		bool RemoveControl (Control control);
		
		// VSBool result, VSInt32 value VSGetDefaultControlValue(VSCString controlType)
		Result<int> GetDefaultControlValue (string controlType);
		
		#endregion

		#region Master Track API
		
		// void VSSeekToBeginTempo()
		void SeekToBeginTempo ();
		
		// void VSSeekToBeginTimeSig()
		void SeekToBeginTimeSig ();
		
		// VSBool result, VSLuaTempo tempo VSGetNextTempo()
		Result<Tempo> GetNextTempo ();
		
		// VSBool result, VSLuaTimeSig timeSig VSGetNextTimeSig()
		Result<TimeSignature> GetNextTimeSignature ();
		
		// VSBool result, VSFloat tempo VSGetTempoAt(VSInt32 posTick)
		Result<float> GetTempoAt (int positionTick);
		
		// VSBool result, VSInt32 numerator, VSInt32 denominator VSGetTimeSigAt(VSInt32 posTick)
		// we need non-void template parameter, so we return tick-pointless TimeSignature here.
		Result<TimeSignature> GetTimeSignatureAt (int positionTick);
		
		// VSCString VSGetSequenceName()
		string GetSequenceName ();
		
		// VSCString VSGetSequencePath()
		string GetSequencePath ();
		
		// VSInt32 VSGetResolution ()
		int GetResolution ();
		
		#endregion

		#region Musical Part API

		// VSBool result, VSLuaMusicalPart part VSGetMusicalPart()
		Result<MusicalPart> GetMusicalPart ();

		// VSBool VSUpdateMusicalPart(VSLuaMusicalPart part)
		bool UpdateMusicalPart (MusicalPart part);

		// VSBool result, VSLuaMusicalSinger VSGetMusicalPartSinger()
		Result<MusicalSinger> GetMusicalPartSinger ();

		#endregion

		#region WAV Part API
		
		// VSBool result, VSLuaWAVPart wavPart VSGetStereoWAVPart()
		Result<WavPart> GetStereoWavPart ();
		
		// void VSSeekToBeginMonoWAVPart()
		void SeekToBeginMonoWavPart ();
		
		// VSBool result, VSLuaWAVPart wavPart VSGetNextMonoWAVPart()
		Result<WavPart> GetNextMonoWavPart ();
		
		// VSInt32 VSMessageBox(VSCString message, VSUInt32 type)
		MessageBoxResult MessageBox (string message, MessageBoxStyle type);
		
		#endregion
	}
}

class MonoTODOAttribute : Attribute
{
	public MonoTODOAttribute (string bah) {}
}

