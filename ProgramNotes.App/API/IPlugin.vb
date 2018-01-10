Option Explicit On
Option Strict On

Namespace ProgramNotes.API

    Public Interface IPlugin

        ''' <summary>Имя плагина, оно же используется как название пункта меню</summary>
        ReadOnly Property Name As String

        ''' <summary>Версия плагина</summary>
        ReadOnly Property Version As String

        ''' <summary>Ссылка на значок плагина отображаемый в меню</summary>
        ReadOnly Property Icon As Image

        ''' <summary>Функция вызывается приложением</summary>
        ''' <param name="Context">Контекст приложения с настройками и объектами</param>
        ''' <returns>Произвольный объект</returns>
        Function DoAction(Context As IContext) As Object

    End Interface

End Namespace