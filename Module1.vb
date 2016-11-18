Imports System.IO

Module Module1

    ReadOnly Machine As New Machine(Machine.State.A, Machine.State.C)
    Dim _stateChoices As Dictionary(Of Machine.State, List(Of String))

    Sub Main()
        Const fileName As String = "MyNachine.txt"

        Machine.SetDefaultAction(AddressOf DefaultAction)

        Machine.AddTransition(Machine.State.A, Machine.State.B, Machine.Trigger.Do1, AddressOf State1Action)
        Machine.AddTransition(Machine.State.B, Machine.State.C, Machine.Trigger.Do2, AddressOf State2Action)
        Machine.AddTransition(Machine.State.C, Machine.State.A, Machine.Trigger.Do3, AddressOf State3Action)

        _stateChoices = LoadStateChoices(fileName)

        Machine.Start()

        Console.WriteLine("Thanks for playing!")
    End Sub

    Private Function LoadStateChoices(ByVal fileName As String) As Dictionary(Of Machine.State, List(Of String))
        Dim stateChoices As New Dictionary(Of Machine.State, List(Of String))

        Dim lines As List(Of String) = File.ReadLines(fileName)

        If lines IsNot Nothing AndAlso lines.Count > 0 AndAlso Not (lines.Count = 0 AndAlso String.IsNullOrEmpty(lines(0))) Then
            For Each line As String In lines
                If Not String.IsNullOrWhiteSpace(line) Then
                    'state A
                    '
                    ' eat ice cream -> B
                    ' eat cake -> C
                End If
            Next
        End If

        Return stateChoices
    End Function

    Sub DefaultAction()
        Dim currState As Machine.State = Machine.GetCurrentState()
        Dim choices As List(Of String) = _stateChoices(currState)
        Dim trigger As Machine.Trigger = PromptChoices(choices)
        Machine.DoTrigger(trigger)
    End Sub

    Sub State1Action()
        Console.WriteLine("This is the beginning")
    End Sub

    Sub State2Action()
        Console.WriteLine("This is the middle")
    End Sub

    Sub State3Action()
        Console.WriteLine("This is the end")
    End Sub

    Function PromptChoices(ByVal choices As List(Of String)) As Machine.Trigger
        Console.WriteLine("Choose one:")

        Dim choicePrompts As List(Of String) = choices.Where(Function(c, i) i Mod 2 <> 0).ToList
        Dim triggerNames As List(Of String) = choices.Where(Function(c, i) i Mod 2 = 0).ToList

        Dim count As Integer = 0
        choicePrompts.ForEach(Sub(prompt)
                                  count = count + 1
                                  Console.WriteLine("{0}. {1}", count, prompt)
                              End Sub)

        Dim val As String = Console.ReadLine()

        While val Is Nothing
            Console.WriteLine("Please choose again:")
            choices.ForEach(Sub(selection) Console.WriteLine(selection))
            val = Console.ReadLine()
        End While

        Dim triggerName As String = triggerNames(Integer.Parse(val) - 1)

        Return [Enum].Parse(GetType(Machine.Trigger), triggerName)
    End Function
End Module
