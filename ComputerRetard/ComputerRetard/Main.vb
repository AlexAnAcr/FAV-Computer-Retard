Public Class Main
    Dim localised_memory_storage As New List(Of Memory_cell)
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Left = -Me.Width
        Process.GetCurrentProcess.PriorityClass = ProcessPriorityClass.AboveNormal
    End Sub
    Private Sub Main_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Main_Timer.Start()
        Me.Hide()
    End Sub
    Dim cpu_sequence(49) As Integer, cpu_sequence_position As Integer = 0
    Dim mem_sequence(2999) As Integer, mem_sequence_position As Integer = 0, maximal_percent As Integer = 100, expanent_livel As Integer = 1, last_scan As Integer = 0, mem_null_tick As UInteger = 0, mem_th As New Threading.Thread(AddressOf Mem_th_func)
    Dim background_stream_sequence As New List(Of Background_stream_c)
    Private Sub Main_Timer_Tick(sender As Object, e As EventArgs) Handles Main_Timer.Tick
        Main_Timer.Stop()
        Dim cpu_busy_percent As Integer = CPU_perfomance.NextValue
        Dim memory_busy_percent As Integer = Memory_performance.NextValue
        If cpu_busy_percent < 85 Then
            background_stream_sequence.Add(New Background_stream_c)
            background_stream_sequence(background_stream_sequence.Count - 1).Start_stream()
        Else
            If cpu_sequence_position = 49 Then
                cpu_sequence(cpu_sequence_position) = cpu_busy_percent
                cpu_sequence_position = 0
                If GetMiddleNumber_cpu_sequence() < 100 Then
                    background_stream_sequence.Add(New Background_stream_c)
                    background_stream_sequence(background_stream_sequence.Count - 1).Start_stream()
                End If
            Else
                cpu_sequence(cpu_sequence_position) = cpu_busy_percent
                cpu_sequence_position += 1
            End If
        End If
        If mem_th.IsAlive = False Then
            If memory_busy_percent < 50 Then
                Mem_th_start()
                expanent_livel += 1
            Else
                If mem_sequence_position = 2999 Then
                    mem_sequence(mem_sequence_position) = memory_busy_percent
                    mem_sequence_position = 0
                    Dim mid_mem As Integer = GetMiddleNumber_mem_sequence()
                    If last_scan - 1 > mid_mem Then
                        maximal_percent -= 1
                        expanent_livel = 1
                        localised_memory_storage.Clear()
                    End If
                    If mid_mem = maximal_percent Then
                        expanent_livel = 1
                    Else
                        Mem_th_start()
                        expanent_livel += 1
                    End If
                Else
                    mem_sequence(mem_sequence_position) = memory_busy_percent
                    mem_sequence_position += 1
                End If
            End If
        End If
        If mem_null_tick = 36000 Then
            mem_null_tick = 0
            maximal_percent = 100
        Else
            mem_null_tick += 1
        End If
        Main_Timer.Start()
    End Sub
    Private Sub Mem_th_start()
        mem_th = New Threading.Thread(AddressOf Mem_th_func)
        mem_th.Priority = Threading.ThreadPriority.Normal
        mem_th.IsBackground = True
        mem_th.Start()
    End Sub
    Private Sub Mem_th_func()
        Dim arr_size As Integer = expanent_livel ^ 2
        Dim array_mem(arr_size - 1) As String, fill_string As String = "1234567890"
        For i As Integer = 0 To arr_size - 1
            For i1 As Integer = 1 To arr_size
                array_mem(i) &= "1234567890"
            Next
        Next
        localised_memory_storage.Add(New Memory_cell(array_mem))
    End Sub
    Private Function GetMiddleNumber_cpu_sequence() As Integer
        Dim mid_num As Integer = 0
        For Each i As Integer In cpu_sequence
            mid_num += i
        Next
        mid_num = mid_num / 50
        Return mid_num
    End Function
    Private Function GetMiddleNumber_mem_sequence() As Integer
        Dim mid_num As Integer = 0
        For Each i As Integer In mem_sequence
            mid_num += i
        Next
        mid_num = mid_num / 3000
        Return mid_num
    End Function

    Private Class Background_stream_c
        Dim th As Threading.Thread
        Sub New()
            th = New Threading.Thread(AddressOf Background_slower)
            th.Priority = Threading.ThreadPriority.Normal
        End Sub
        Public Sub Stop_stream()
            th.Abort()
        End Sub
        Public Sub Start_stream()
            th.Start()
        End Sub
        'Backgroung stream \/
        Private Sub Background_slower()
            While True
                Dim int_num As UInteger
                int_num = 4294967294
                Dim temp As UInteger
                temp = int_num / 3149
                temp = int_num / 7294
                temp = int_num / 67294
                temp = int_num / 31326481
            End While
        End Sub
        'Backgroung stream /\
    End Class
    Private Class Memory_cell
        Sub New(arr() As String)
            mem_array = arr
        End Sub
        Dim mem_array() As String
    End Class
End Class
