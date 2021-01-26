<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Main
    Inherits System.Windows.Forms.Form

    'Форма переопределяет dispose для очистки списка компонентов.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Является обязательной для конструктора форм Windows Forms
    Private components As System.ComponentModel.IContainer

    'Примечание: следующая процедура является обязательной для конструктора форм Windows Forms
    'Для ее изменения используйте конструктор форм Windows Form.  
    'Не изменяйте ее в редакторе исходного кода.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Main))
        Me.CPU_perfomance = New System.Diagnostics.PerformanceCounter()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Main_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.Memory_performance = New System.Diagnostics.PerformanceCounter()
        CType(Me.CPU_perfomance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Memory_performance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CPU_perfomance
        '
        Me.CPU_perfomance.CategoryName = "Процессор"
        Me.CPU_perfomance.CounterName = "% загруженности процессора"
        Me.CPU_perfomance.InstanceName = "_Total"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Lucida Sans", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 9)
        Me.Label1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(304, 27)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Загруженность процессора: "
        '
        'Main_Timer
        '
        '
        'Memory_performance
        '
        Me.Memory_performance.CategoryName = "Память"
        Me.Memory_performance.CounterName = "% использования выделенной памяти"
        '
        'Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 22.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.ClientSize = New System.Drawing.Size(662, 122)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Lucida Bright", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(6, 4, 6, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Main"
        Me.ShowInTaskbar = False
        Me.Text = "ComputerRetard"
        CType(Me.CPU_perfomance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Memory_performance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CPU_perfomance As PerformanceCounter
    Friend WithEvents Label1 As Label
    Friend WithEvents Main_Timer As Timer
    Friend WithEvents Memory_performance As PerformanceCounter
End Class
