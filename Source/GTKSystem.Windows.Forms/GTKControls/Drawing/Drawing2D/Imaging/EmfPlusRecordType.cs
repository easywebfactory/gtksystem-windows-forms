namespace System.Drawing.Imaging
{
	/// <summary>Specifies the methods available for use with a metafile to read and write graphic commands.</summary>
	public enum EmfPlusRecordType
	{
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfRecordBase = 0x10000,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetBkColor = 66049,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetBkMode = 65794,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetMapMode = 65795,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetROP2 = 65796,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetRelAbs = 65797,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetPolyFillMode = 65798,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetStretchBltMode = 65799,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetTextCharExtra = 65800,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetTextColor = 66057,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetTextJustification = 66058,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetWindowOrg = 66059,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetWindowExt = 66060,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetViewportOrg = 66061,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetViewportExt = 66062,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfOffsetWindowOrg = 66063,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfScaleWindowExt = 66576,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfOffsetViewportOrg = 66065,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfScaleViewportExt = 66578,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfLineTo = 66067,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfMoveTo = 66068,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfExcludeClipRect = 66581,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfIntersectClipRect = 66582,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfArc = 67607,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfEllipse = 66584,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfFloodFill = 66585,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPie = 67610,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfRectangle = 66587,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfRoundRect = 67100,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPatBlt = 67101,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSaveDC = 65566,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetPixel = 66591,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfOffsetCilpRgn = 66080,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfTextOut = 66849,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfBitBlt = 67874,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfStretchBlt = 68387,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPolygon = 66340,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPolyline = 66341,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfEscape = 67110,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfRestoreDC = 65831,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfFillRegion = 66088,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfFrameRegion = 66601,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfInvertRegion = 65834,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPaintRegion = 65835,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSelectClipRegion = 65836,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSelectObject = 65837,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetTextAlign = 65838,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfChord = 67632,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetMapperFlags = 66097,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfExtTextOut = 68146,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetDibToDev = 68915,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSelectPalette = 66100,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfRealizePalette = 65589,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfAnimatePalette = 66614,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetPalEntries = 65591,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfPolyPolygon = 66872,
		/// <summary>Increases or decreases the size of a logical palette based on the specified value.</summary>
		WmfResizePalette = 65849,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfDibBitBlt = 67904,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfDibStretchBlt = 68417,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfDibCreatePatternBrush = 65858,
		/// <summary>Copies the color data for a rectangle of pixels in a DIB to the specified destination rectangle.</summary>
		WmfStretchDib = 69443,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfExtFloodFill = 66888,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfSetLayout = 65865,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfDeleteObject = 66032,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreatePalette = 65783,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreatePatternBrush = 66041,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreatePenIndirect = 66298,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreateFontIndirect = 66299,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreateBrushIndirect = 66300,
		/// <summary>See "Windows-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		WmfCreateRegion = 67327,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfHeader = 1,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyBezier = 2,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolygon = 3,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyline = 4,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyBezierTo = 5,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyLineTo = 6,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyPolyline = 7,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyPolygon = 8,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetWindowExtEx = 9,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetWindowOrgEx = 10,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetViewportExtEx = 11,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetViewportOrgEx = 12,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetBrushOrgEx = 13,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfEof = 14,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetPixelV = 0xF,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetMapperFlags = 0x10,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetMapMode = 17,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetBkMode = 18,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetPolyFillMode = 19,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetROP2 = 20,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetStretchBltMode = 21,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetTextAlign = 22,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetColorAdjustment = 23,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetTextColor = 24,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetBkColor = 25,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfOffsetClipRgn = 26,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfMoveToEx = 27,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetMetaRgn = 28,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExcludeClipRect = 29,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfIntersectClipRect = 30,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfScaleViewportExtEx = 0x1F,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfScaleWindowExtEx = 0x20,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSaveDC = 33,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfRestoreDC = 34,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetWorldTransform = 35,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfModifyWorldTransform = 36,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSelectObject = 37,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreatePen = 38,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreateBrushIndirect = 39,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfDeleteObject = 40,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfAngleArc = 41,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfEllipse = 42,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfRectangle = 43,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfRoundRect = 44,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfRoundArc = 45,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfChord = 46,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPie = 47,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSelectPalette = 48,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreatePalette = 49,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetPaletteEntries = 50,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfResizePalette = 51,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfRealizePalette = 52,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtFloodFill = 53,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfLineTo = 54,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfArcTo = 55,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyDraw = 56,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetArcDirection = 57,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetMiterLimit = 58,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfBeginPath = 59,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfEndPath = 60,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCloseFigure = 61,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfFillPath = 62,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfStrokeAndFillPath = 0x3F,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfStrokePath = 0x40,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfFlattenPath = 65,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfWidenPath = 66,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSelectClipPath = 67,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfAbortPath = 68,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfReserved069 = 69,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfGdiComment = 70,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfFillRgn = 71,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfFrameRgn = 72,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfInvertRgn = 73,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPaintRgn = 74,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtSelectClipRgn = 75,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfBitBlt = 76,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfStretchBlt = 77,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfMaskBlt = 78,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPlgBlt = 79,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetDIBitsToDevice = 80,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfStretchDIBits = 81,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtCreateFontIndirect = 82,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtTextOutA = 83,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtTextOutW = 84,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyBezier16 = 85,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolygon16 = 86,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyline16 = 87,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyBezierTo16 = 88,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolylineTo16 = 89,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyPolyline16 = 90,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyPolygon16 = 91,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyDraw16 = 92,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreateMonoBrush = 93,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreateDibPatternBrushPt = 94,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtCreatePen = 95,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyTextOutA = 96,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPolyTextOutW = 97,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetIcmMode = 98,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreateColorSpace = 99,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetColorSpace = 100,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfDeleteColorSpace = 101,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfGlsRecord = 102,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfGlsBoundedRecord = 103,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPixelFormat = 104,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfDrawEscape = 105,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfExtEscape = 106,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfStartDoc = 107,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSmallTextOut = 108,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfForceUfiMapping = 109,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfNamedEscpae = 110,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfColorCorrectPalette = 111,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetIcmProfileA = 112,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetIcmProfileW = 113,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfAlphaBlend = 114,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetLayout = 115,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfTransparentBlt = 116,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfReserved117 = 117,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfGradientFill = 118,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetLinkedUfis = 119,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfSetTextJustification = 120,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfColorMatchToTargetW = 121,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfCreateColorSpaceW = 122,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfMax = 122,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfMin = 1,
		/// <summary>See "Enhanced-Format Metafiles" in the GDI section of the MSDN Library.</summary>
		EmfPlusRecordBase = 0x4000,
		/// <summary>Indicates invalid data.</summary>
		Invalid = 0x4000,
		/// <summary>Identifies a record that is the EMF+ header.</summary>
		Header = 16385,
		/// <summary>Identifies a record that marks the last EMF+ record of a metafile.</summary>
		EndOfFile = 16386,
		/// <summary>See <see cref="M:System.Drawing.Graphics.AddMetafileComment(System.Byte[])" />.</summary>
		Comment = 16387,
		/// <summary>See <see cref="M:System.Drawing.Graphics.GetHdc" />.</summary>
		GetDC = 16388,
		/// <summary>Marks the start of a multiple-format section.</summary>
		MultiFormatStart = 16389,
		/// <summary>Marks a multiple-format section.</summary>
		MultiFormatSection = 16390,
		/// <summary>Marks the end of a multiple-format section.</summary>
		MultiFormatEnd = 16391,
		/// <summary>Marks an object.</summary>
		Object = 16392,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Clear(System.Drawing.Color)" />.</summary>
		Clear = 16393,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillRectangles" /> methods.</summary>
		FillRects = 16394,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawRectangles" /> methods.</summary>
		DrawRects = 16395,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillPolygon" /> methods.</summary>
		FillPolygon = 16396,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawLines" /> methods.</summary>
		DrawLines = 16397,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillEllipse" /> methods.</summary>
		FillEllipse = 16398,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawEllipse" /> methods.</summary>
		DrawEllipse = 16399,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillPie" /> methods.</summary>
		FillPie = 16400,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawPie" /> methods.</summary>
		DrawPie = 16401,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawArc" /> methods.</summary>
		DrawArc = 16402,
		/// <summary>See <see cref="M:System.Drawing.Graphics.FillRegion(System.Drawing.Brush,System.Drawing.Region)" />.</summary>
		FillRegion = 16403,
		/// <summary>See <see cref="M:System.Drawing.Graphics.FillPath(System.Drawing.Brush,System.Drawing.Drawing2D.GraphicsPath)" />.</summary>
		FillPath = 16404,
		/// <summary>See <see cref="M:System.Drawing.Graphics.DrawPath(System.Drawing.Pen,System.Drawing.Drawing2D.GraphicsPath)" />.</summary>
		DrawPath = 16405,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.FillClosedCurve" /> methods.</summary>
		FillClosedCurve = 16406,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawClosedCurve" /> methods.</summary>
		DrawClosedCurve = 16407,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawCurve" /> methods.</summary>
		DrawCurve = 16408,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawBeziers" /> methods.</summary>
		DrawBeziers = 16409,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawImage" /> methods.</summary>
		DrawImage = 16410,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawImage" /> methods.</summary>
		DrawImagePoints = 16411,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.DrawString" /> methods.</summary>
		DrawString = 16412,
		/// <summary>See <see cref="P:System.Drawing.Graphics.RenderingOrigin" />.</summary>
		SetRenderingOrigin = 16413,
		/// <summary>See <see cref="P:System.Drawing.Graphics.SmoothingMode" />.</summary>
		SetAntiAliasMode = 16414,
		/// <summary>See <see cref="P:System.Drawing.Graphics.TextRenderingHint" />.</summary>
		SetTextRenderingHint = 16415,
		/// <summary>See <see cref="P:System.Drawing.Graphics.TextContrast" />.</summary>
		SetTextContrast = 16416,
		/// <summary>See <see cref="P:System.Drawing.Graphics.InterpolationMode" />.</summary>
		SetInterpolationMode = 16417,
		/// <summary>See <see cref="P:System.Drawing.Graphics.PixelOffsetMode" />.</summary>
		SetPixelOffsetMode = 16418,
		/// <summary>See <see cref="P:System.Drawing.Graphics.CompositingMode" />.</summary>
		SetCompositingMode = 16419,
		/// <summary>See <see cref="P:System.Drawing.Graphics.CompositingQuality" />.</summary>
		SetCompositingQuality = 16420,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Save" />.</summary>
		Save = 16421,
		/// <summary>See <see cref="M:System.Drawing.Graphics.Restore(System.Drawing.Drawing2D.GraphicsState)" />.</summary>
		Restore = 16422,
		/// <summary>See <see cref="M:System.Drawing.Graphics.BeginContainer" /> methods.</summary>
		BeginContainer = 16423,
		/// <summary>See <see cref="M:System.Drawing.Graphics.BeginContainer" /> methods.</summary>
		BeginContainerNoParams = 16424,
		/// <summary>See <see cref="M:System.Drawing.Graphics.EndContainer(System.Drawing.Drawing2D.GraphicsContainer)" />.</summary>
		EndContainer = 16425,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		SetWorldTransform = 16426,
		/// <summary>See <see cref="M:System.Drawing.Graphics.ResetTransform" />.</summary>
		ResetWorldTransform = 16427,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.MultiplyTransform" /> methods.</summary>
		MultiplyWorldTransform = 16428,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		TranslateWorldTransform = 16429,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.ScaleTransform" /> methods.</summary>
		ScaleWorldTransform = 16430,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.RotateTransform" /> methods.</summary>
		RotateWorldTransform = 16431,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TransformPoints" /> methods.</summary>
		SetPageTransform = 16432,
		/// <summary>See <see cref="M:System.Drawing.Graphics.ResetClip" />.</summary>
		ResetClip = 16433,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		SetClipRect = 16434,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		SetClipPath = 16435,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.SetClip" /> methods.</summary>
		SetClipRegion = 16436,
		/// <summary>See <see cref="Overload:System.Drawing.Graphics.TranslateClip" /> methods.</summary>
		OffsetClip = 16437,
		/// <summary>Specifies a character string, a location, and formatting information.</summary>
		DrawDriverString = 16438,
		/// <summary>Used internally.</summary>
		Total = 16439,
		/// <summary>The maximum value for this enumeration.</summary>
		Max = 16438,
		/// <summary>The minimum value for this enumeration.</summary>
		Min = 16385
	}
}
