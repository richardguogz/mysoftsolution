// This is the C# implementation of Data Binding PictureBox converted from 
// VB .NET by LZF of www.codeproject.com. The original code and article, 
// writen by Duncan Mackenzie of MSDN, can be found at
// "http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwinforms/html/custcntrlsamp3.asp"
//
// Following block is the copyright and the description from the original code

/*
'Copyright (C) 2002 Microsoft Corporation
'All rights reserved.
'
'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER
'EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF
'MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'
'Date: May 2002
'Author: Duncan Mackenzie
'
'Requires the release version of .NET Framework

'This control allows you to provide a file path
'to its ImagePath property and have it load the image
'
'Having this ImagePath property allows the control
'to be databound, therefore supporting the display
'of a picture based on a path stored in a database.

'thinking that missing files and bad images would be
'common, I am handling those errors simply by 
'clearing the image. That way you won't get an exception
'but you also won't see the last record's image
'with the current record's data.

'If you would rather have an exception occur,
'simply rewrite the ImagePath property and the 
'UpdateImage functions to remove the Exists check 
'and the Try Catch... like this;

'Public Property ImagePath() As String
'    Get
'        Return m_ImagePath
'    End Get
'    Set(ByVal Value As String)
'        If Value <> m_ImagePath Then
'            m_ImagePath = Value
'            UpdateImage()
'            RaiseEvent ImagePathChanged(CObj(Me), New EventArgs())
'        End If
'    End Set
'End Property

'Private Sub UpdateImage()
'    Me.Image = New Bitmap(ImagePath)
'End Sub
*/
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace ControlLib
{
	/// <summary>
	/// Summary description for dbImageBox.
	/// </summary>
	public class dbImageBox : System.Windows.Forms.PictureBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string m_ImagePath;

//		public delegate void ImgEventHandler (object s);
//		public event ImgEventHandler ImagePathChanged;

		public dbImageBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		protected override void OnPaint(PaintEventArgs pe)
		{
			// TODO: Add custom paint code here

			// Calling the base class OnPaint
			base.OnPaint(pe);
		}

		public string ImagePath
		{
			get
			{
				return this.m_ImagePath;
			}
			set
			{
				if ( value != this.m_ImagePath )
				{
					this.m_ImagePath = value;
					if ( System.IO.File.Exists(value) )
						UpdateImage();
					else
						this.Image = null;
//					ImagePathChanged(this);
				}
			}
		}

		public void UpdateImage()
		{
			try
			{
				this.Image = Image.FromFile(ImagePath);
			}
			catch (System.Exception e)
			{
				MessageBox.Show (e.Message);
				this.Image = null;
			}
		}
		


		
	}
}
