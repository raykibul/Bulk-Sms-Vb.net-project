Imports System.IO
Imports System.IO.Ports
Imports System.Threading


Public Class Form1

    Dim counter As Integer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            Dim ports As String() = SerialPort.GetPortNames
            Dim port As String
            For Each port In ports
                ComboBox1.Items.Add(port)
            Next port
            ComboBox1.SelectedIndex = 0
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub selectbtn_Click(sender As Object, e As EventArgs) Handles selectbtn.Click
        Try
            With myport
                .PortName = ComboBox1.Text
                .BaudRate = 115200
                .Parity = Parity.None
                .StopBits = StopBits.One
                .DataBits = 8
                .Handshake = Handshake.RequestToSend
                .DtrEnable = True
                .RtsEnable = True
                .NewLine = vbCrLf
                .Open()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Sub
    Private Sub sendmsg(number As String)
        Try
            If myport.IsOpen Then

                With myport
                    .Write("AT" & vbCrLf)
                    Thread.Sleep(500)
                    .Write("AT+CMGF=1" & vbCrLf)
                    Thread.Sleep(600)
                    .Write("AT+CMGS=" & Chr(34) & number & Chr(34) & vbCrLf)
                    Thread.Sleep(600)

                    .Write(RichTextBox1.Text & "Sms No: " & counter & "now Time:" & Now.ToLongTimeString & Chr(26))

                    counter += 1
                    total.Text = " TOTAL SENT :" & counter

                    Debug.WriteLine("SMS Sent TO : " & number)
                    Debug.WriteLine("Current Time: ")
                    Debug.WriteLine(Now.ToLongTimeString)


                End With
            Else
                MsgBox("Error on the port selected")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try



    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        counter = 0

        If myworker.IsBusy Then
            statetext.Text = "NETWORK SLOW"
            statetext.ForeColor = Color.IndianRed

        Else


            myworker.RunWorkerAsync()
            statetext.Text = "NETWORK OKAY"
            statetext.ForeColor = Color.Green


        End If


    End Sub

    Private Sub myworker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles myworker.DoWork
        Control.CheckForIllegalCrossThreadCalls = False

        Dim num As String
        num = TextBox1.Text
        Dim numbers As String() = num.Split(New Char() {","c})
        Dim number As String
        For Each number In numbers
            Thread.Sleep(300)
            Debug.WriteLine(number)
            sendmsg(number)

        Next
        Debug.WriteLine("LOOP ENDS")


    End Sub


End Class
