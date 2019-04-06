namespace BTCGatewayAPI.Infrastructure.Logging
{
	/// <summary>
	/// Уровни событий уведомления
	/// </summary>
	public enum LogEntryTypeLevel
	{
		#region Common logging

		/// <summary>
		/// Отладка
		/// </summary>
		Debug = 0,
		/// <summary>
		/// Информация
		/// </summary>
		Info = 1,
		/// <summary>
		/// Предупреждение
		/// </summary>
		Warn = 2,
		/// <summary>
		/// Ошибка
		/// </summary>
		Error = 3,
		/// <summary>
		/// Критическая ошибка
		/// </summary>
		Fatal = 4,

		/// <summary>
		/// Трасировка
		/// </summary>
		Trace = 5,

		/// <summary>
		/// Отключено
		/// </summary>
		Off = 6

		#endregion
	}
}
