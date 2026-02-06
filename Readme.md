# Ini.Net
Высокопроизводительный парсер ini-файлов.

# Пример использования
```
using Ini.ReadOnly;

string filePath = *Путь до файла*;

ReadOnlyIniFile file 	= ReadOnlyIniFile.Read(filePath);
string value1 			= file["section1", "key1"]; //Получаем значение сразу из файла

ReadOnlyIniSection section 	= file["section1"];
string value2 				= section["key2"]; //Получаем значение из секции
```

# Тесты производительности
Парсинг файла на 100000 значений:

| Method                 | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|----------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| ini-parser-netstandard | 164.38 ms |  3.254 ms |  8.629 ms | 8000.0000 | 5333.3333 | 2000.0000 |  68.98 MB |
| PeanutButter.INI       | 545.04 ms | 10.760 ms | 10.568 ms | 8000.0000 | 4000.0000 | 2000.0000 |  67.01 MB |
| Ini.Net            	 |  53.97 ms |  1.061 ms |  2.044 ms | 3888.8889 | 2555.5556 | 1111.1111 |  32.43 MB |
