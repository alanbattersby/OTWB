Public Structure recordtype1
    Public Vnum As Single
    Public Vden As Single
    Public InOut1 As Single
    Public Ex As Single
    Public SR As Single
    Public PHI1 As Single

    Public Overrides Function ToString() As String
        Return String.Format("Vnum ={0}, Vden ={1} ,InOut ={2} ,Ex ={3} Phi1 = {4} ,SR ={5}",
                                   Vnum, Vden, InOut1, Ex, PHI1, SR)
    End Function

    Public Function RawData() As String
        Return String.Format("{0},{1},{2},{3},{4},{5}", Vnum, Vden, InOut1, Ex, PHI1, SR)
    End Function
    Public Function RawData2() As String
        Return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                             Vnum, Vden, InOut1, Ex, PHI1, SR, 0, 0, 0, 0, 0)
    End Function
End Structure
Public Structure recordtype2
    Public V1num As Single
    Public V1den As Single
    Public InOut1 As Single
    Public V2num As Single
    Public V2den As Single
    Public InOut2 As Single
    Public Ex1 As Single
    Public Ex2 As Single
    Public SR As Single
    Public PHI1 As Single
    Public PHI2 As Single

    Public Overrides Function ToString() As String
        Return String.Format("V1num ={0},V1den ={1},InOut1 ={2},Ex1 ={3},Phi1 ={4},SR ={5},V2num ={6},V2den ={7},InOut2 ={8},Ex2 ={9},Phi2 ={10}",
                                  V1num, V1den, InOut1, Ex1, PHI1, SR, V2num, V2den, InOut2, Ex2, PHI2)
    End Function

    Public Function RawData() As String
        Return String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                                V1num, V1den, InOut1, Ex1, PHI1, SR, V2num, V2den, InOut2, Ex2, PHI2)
    End Function

    Public Sub New(ByRef line As String)
        Dim data As String() = line.Split(",")
        If (data.Count >= 11) Then
            V1num = Decimal.Parse(data(0))
            V1den = Decimal.Parse(data(1))
            InOut1 = Decimal.Parse(data(2))
            Ex1 = Decimal.Parse(data(3))
            PHI1 = Decimal.Parse(data(4))
            SR = Decimal.Parse(data(5))
            V2num = Decimal.Parse(data(6))
            V2den = Decimal.Parse(data(7))
            InOut2 = Decimal.Parse(data(8))
            Ex2 = Decimal.Parse(data(9))
            PHI2 = Decimal.Parse(data(10))
        End If
    End Sub

    Public Sub New(ByVal obj As recordtype2)
        V1num = obj.V1num
        V1den = obj.V1den
        InOut1 = obj.InOut1
        Ex1 = obj.Ex1
        PHI1 = obj.PHI1
        SR = obj.SR
        V2num = obj.V2num
        V2den = obj.V2den
        InOut2 = obj.InOut2
        Ex2 = obj.Ex2
        PHI2 = obj.PHI2
    End Sub

    Public Sub New(ByVal obj As BazleyData)
        SR = obj.SR
        If obj.Stages.Count > 0 Then
            V1num = obj.Stage1.Numerator
            V1den = obj.Stage1.Denominator
            InOut1 = obj.Stage1.InOut
            Ex1 = obj.Stage1.Eccentricity
            PHI1 = obj.Stage1.Phase
            If obj.Stages.Count > 1 Then
                V2num = obj.Stage2.Numerator
                V2den = obj.Stage2.Denominator
                InOut2 = obj.Stage2.InOut
                Ex2 = obj.Stage2.Eccentricity
                PHI2 = obj.Stage2.Phase
            Else
                V2num = 1
                V2den = 1
                InOut2 = 1
                Ex2 = 0
                PHI2 = 0
            End If
        Else
            V1num = 1
            V1den = 1
            InOut1 = 1
            Ex1 = 0
            PHI1 = 0
            V2num = 1
            V2den = 1
            InOut2 = 1
            Ex2 = 0
            PHI2 = 0
        End If
    End Sub
End Structure

Public Structure RossData
    Public Ex1 As Decimal
    Public Ex2 As Decimal
    Public SR As Decimal
    Public Fl As Decimal
    Public Fr As Decimal
    Public N As Decimal
    Public M As Decimal
    Public L As Decimal
    Public K As Decimal
    Public V4 As Decimal
    Public Phi1 As Decimal
    Public Phi2 As Decimal
    Public Phi3 As Decimal

End Structure
Public Class RossTable
    Public Function Value(ByVal index As Integer)
        Dim val As RossData = New RossData()
        Select Case index
            Case 3455
                val.Ex1 = 70 : val.SR = 20 : val.Fl = 12 : val.Fr = -22 : val.N = -2 : val.M = 159 : val.L = -161
            Case 3456
                val.Ex1 = 70 : val.SR = 20 : val.Fl = 12 : val.Fr = 22 : val.N = -2 : val.M = 159 : val.L = -161
            Case 3457
                val.Ex1 = 60 : val.SR = 60 : val.Fl = 12 : val.Fr = -27 : val.N = -2 : val.M = -121 : val.L = 119
                val.Phi1 = 60 : val.Phi2 = 60 : val.Phi3 = 60 : val.Ex2 = 0 : val.M = 0 : val.V4 = 0
                'call RossCalculate1
                val.Phi1 = 180 : val.Phi2 = 180 : val.Phi3 = 180 : val.Ex2 = 0 : val.M = 0 : val.V4 = 0
                'call RossCalculate1
                val.Phi1 = 300 : val.Phi2 = 300 : val.Phi3 = 300 : val.Ex2 = 0 : val.M = 0 : val.V4 = 0
            Case 3458
                val.Ex1 = 60 : val.SR = 10 : val.Fl = 10 : val.Fr = -9 : val.N = -2 : val.M = -13 : val.L = 11
                val.Phi1 = 180 : val.Phi2 = 180 : val.Phi3 = 180
            Case 3459
                val.Ex1 = 60 : val.SR = 10 : val.Fl = 10 : val.Fr = -9 : val.N = -2 : val.M = -13 : val.L = 11
                val.Phi1 = 180
            Case 3460
                val.Ex1 = 60 : val.SR = 10 : val.Fl = 10 : val.Fr = 9 : val.N = -2 : val.M = -13 : val.L = 11
                val.Phi1 = 180 : val.Phi2 = 180
            Case 3461
                val.Ex1 = 60 : val.SR = 10 : val.Fl = 10 : val.Fr = 9 : val.N = -2 : val.M = -13 : val.L = 11
                val.Phi1 = 180
            Case 3462
                val.Ex1 = 75 : val.SR = 45 : val.Fl = 10 : val.Fr = -23 : val.N = -3.5 : val.M = 202.5 : val.L = -207.5
                val.Phi1 = 135 : val.Phi2 = 135 : val.Phi3 = 135
            Case 3463
                val.Ex1 = 90 : val.SR = 10 : val.Fl = 12 : val.Fr = -20 : val.N = -4 : val.M = -189 : val.L = 183
            Case 3464
                val.Ex1 = 75 : val.SR = 40 : val.Fl = 12 : val.Fr = 18 : val.N = -4 : val.M = -253 : val.L = 247
            Case 3465
                val.Ex1 = 80 : val.SR = 11 : val.Fl = 23 : val.Fr = -20 : val.N = -4 : val.M = -131 : val.L = 253
                val.Phi2 = 90
            Case 3466
                val.Ex1 = 80 : val.SR = 11 : val.Fl = 23 : val.Fr = -20 : val.N = -4 : val.M = -131 : val.L = 253
                val.Phi1 = 180 : val.Phi2 = 180 : val.Phi3 = 180
            Case 3467
                val.Ex1 = 75 : val.SR = 12 : val.Fl = 25 : val.Fr = -14 : val.N = -4 : val.M = -131 : val.L = 189
                val.Phi1 = 180 : val.Phi2 = 180 : val.Phi3 = 180
            Case 3468
                val.Ex1 = 70 : val.SR = 9 : val.Fl = 40 : val.Fr = -10 : val.N = -4 : val.M = 189 : val.L = -195
                val.Phi1 = 180 : val.Phi3 = 180
            Case 3469
                val.Ex1 = 80 : val.SR = 13 : val.Fl = 25 : val.Fr = -6 : val.N = -4 : val.M = 189 : val.L = 195
            Case 3470
                val.Ex1 = 100 : val.SR = 10 : val.Fl = 12 : val.Fr = -12 : val.N = 8 : val.M = 375 : val.L = -357
                val.Phi1 = 180 : val.Phi2 = 180 : val.Phi3 = 180
            Case 3471
                val.Ex1 = 100 : val.SR = 12 : val.Fl = 12 : val.Fr = 11 : val.N = 8 : val.M = 375 : val.L = -357
            Case 3472
                val.Ex1 = 90 : val.SR = 14 : val.Fl = 15 : val.Fr = -14 : val.N = -8 : val.M = 313 : val.L = -327
            Case 3473
                val.Ex1 = 100 : val.SR = 9 : val.Fl = 12 : val.Fr = 11 : val.N = -8 : val.M = 313 : val.L = -327
            Case 3474
                val.Ex1 = 100 : val.SR = 50 : val.Fl = -19 : val.Fr = -7 : val.N = 9 : val.M = -17 : val.L = 1000
            Case 3475
                val.Ex1 = 85 : val.SR = 40 : val.Fl = 25 : val.Fr = 6 : val.N = -6 : val.M = 19 : val.L = 1000
            Case 3476
                val.Ex1 = 90 : val.SR = 20 : val.Fl = 25 : val.Fr = -15 : val.N = -18 : val.M = 19 : val.L = 1000
            Case 3477
                val.Ex1 = 90 : val.SR = 20 : val.Fl = 30 : val.Fr = 20 : val.N = 15 : val.M = -14 : val.L = 600
            Case 3478
                val.Ex1 = 17 : val.SR = 34 : val.Fl = 34 : val.Fr = 9 : val.N = 2 : val.M = -1 : val.L = 192
            Case 3479  ' here we start with RossTables2 in original code
                val.Ex2 = 17 : val.Ex1 = 34 : val.SR = 34 : val.Fl = 12 : val.Fr = 12
                val.V4 = 2 : val.M = -1 : val.L = 189 : val.K = -191
            Case 3480
                val.Ex2 = 17 : val.Ex1 = 34 : val.SR = 34 : val.Fl = 12 : val.Fr = -12
                val.V4 = 2 : val.M = -1 : val.L = 189 : val.K = -191
            Case 3481
                val.Ex2 = 80 : val.Ex1 = 16 : val.SR = 10 : val.Fl = 9 : val.Fr = 7
                val.V4 = -2 : val.M = 11 : val.L = 469 : val.K = -447
            Case 3482 : Return val
            Case 3483 : Return val
            Case 3484 : Return val
            Case 3485 : Return val
            Case 3486
                val.Ex2 = 80 : val.Ex1 = 30 : val.SR = 11 : val.Fl = -10 : val.Fr = 0
                val.V4 = -8 : val.M = 249 : val.L = -247 : val.K = 0
            Case 3487   'Rotate
                val.Ex2 = 80 : val.Ex1 = 21 : val.SR = 19 : val.Fl = 20 : val.Fr = 0
                val.V4 = -8 : val.M = 249 : val.L = -247 : val.K = 0
            Case 3488   'Rotate
                val.Ex2 = 80 : val.Ex1 = 21 : val.SR = 19 : val.Fl = -20 : val.Fr = 0
                val.V4 = -7 : val.M = 330 : val.L = -321 : val.K = 0
            Case 3489   'Rotate
                val.Ex2 = 70 : val.Ex1 = 13 : val.SR = 20 : val.Fl = 23 : val.Fr = 0
                val.V4 = -5 : val.M = 236 : val.L = -229 : val.K = 0
            Case 3490   'Rotate
                val.Ex2 = 80 : val.Ex1 = 32 : val.SR = 19 : val.Fl = -18 : val.Fr = 0
                val.V4 = 8 : val.M = -375 : val.L = 369 : val.K = 0
            Case 3491   'Rotate
                val.Ex2 = 75 : val.Ex1 = 30 : val.SR = 18 : val.Fl = 17 : val.Fr = 0
                val.V4 = -6 : val.M = 379 : val.L = -365 : val.K = 0
            Case 3492   'Rotate
                val.Ex2 = 80 : val.Ex1 = 32 : val.SR = 8 : val.Fl = -14 : val.Fr = 0
                val.V4 = 5 : val.M = -314 : val.L = 306 : val.K = 0
            Case 3493   'Rotate
                val.Ex2 = 70 : val.Ex1 = 35 : val.SR = 9 : val.Fl = 18 : val.Fr = 0
                val.V4 = -6 : val.M = 251 : val.L = -245 : val.K = 0
            Case 3494   'Rotate
                val.Ex2 = 80 : val.Ex1 = 35 : val.SR = 11 : val.Fl = -12 : val.Fr = 0
                val.V4 = 6 : val.M = 331 : val.L = -299 : val.K = 0
            Case 3495   'Rotate
                val.Ex2 = 80 : val.Ex1 = 40 : val.SR = 10 : val.Fl = 11 : val.Fr = 0
                val.V4 = 6 : val.M = 331 : val.L = -299 : val.K = 0
            Case 3496 : Return val
            Case 3497 : Return val
            Case 3498 : Return val
            Case 3499 'Try again
                val.Ex2 = 95 : val.Ex1 = 30 : val.SR = 30 : val.Fl = 8 : val.Fr = 7
                val.V4 = -8 : val.M = 13 : val.L = 662 : val.K = -588
            Case 3500
                val.Ex2 = 96 : val.Ex1 = 36 : val.SR = 30 : val.Fl = 9 : val.Fr = -8
                val.V4 = -8 : val.M = 11 : val.L = 662 : val.K = -592
        End Select
        Return val
    End Function
End Class

   