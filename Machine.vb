Public Class Machine
    Public Enum State
        A
        B
        C
    End Enum

    Public Enum Trigger
        Do1
        Do2
        Do3
        Do4
        Do5
    End Enum

    Private ReadOnly _graph As Dictionary(Of GraphKey, StateAction)
    Private _currentState As State
    Private _currentTrigger As Trigger
    Private ReadOnly _endState As State
    Private _defaultAction As Action

    Public Sub New(ByVal initialState As State, ByVal finalState As State)
        _graph = New Dictionary(Of GraphKey, StateAction)
        _currentState = initialState
        _endState = finalState
    End Sub

    Public Sub SetDefaultAction(ByVal defaultAction As Action)
        _defaultAction = defaultAction
    End Sub

    Public Function GetCurrentState() As State
        Return _currentState
    End Function

    Private Sub InvokeTrigger(ByVal trigger As Trigger)
        Dim key As New GraphKey(_currentState, trigger)
        Dim val As StateAction = _graph(key)

        _currentState = val.GetState

        val.Invoke()

        If _defaultAction IsNot Nothing Then _defaultAction.Invoke()
    End Sub

    Public Sub DoTrigger(ByVal trigger As Trigger)
        _currentTrigger = trigger
    End Sub

    Public Sub Start()
        While _currentState <> _endState
            InvokeTrigger(_currentTrigger)
        End While
    End Sub

    Public Sub AddTransition(ByVal fromState As State, ByVal toState As State, ByVal trigger As Trigger, ByVal action As Action)
        Dim key As New GraphKey(fromState, trigger)
        Dim val As New StateAction(toState, action)
        _graph.Add(key, val)
    End Sub

    Private Class StateAction
        Private ReadOnly _action As Action
        Private ReadOnly _state As State

        Public Sub New(ByVal state As State, ByVal action As Action)
            _state = state
            _action = action
        End Sub

        Public Function GetState() As State
            Return _state
        End Function

        Public Sub Invoke()
            _action.Invoke()
        End Sub
    End Class

    Private Class GraphKey
        Private ReadOnly _fromState As State
        Private ReadOnly _trigger As Trigger

        Public Sub New(ByVal fromState As State, ByVal trigger As Trigger)
            _fromState = fromState
            _trigger = trigger
        End Sub

        Public Overrides Function GetHashCode() As Integer
            Return (_fromState.ToString & _trigger.ToString).GetHashCode
        End Function

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If obj Is Nothing OrElse Me.GetType IsNot obj.GetType Then Return False

            Dim other As GraphKey = DirectCast(obj, GraphKey)

            Return _fromState.Equals(other._fromState) AndAlso _trigger.Equals(other._trigger)
        End Function
    End Class
End Class
