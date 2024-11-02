## CodeNet.Messaging

CodeNet.Messaging is a .Net library.

### Installation

Use the package manager [npm](https://www.nuget.org/packages/CodeNet.Messaging/) to install CodeNet.Messaging

```bash
dotnet add package CodeNet.Messaging
```

### Usage

Param
```json
{
    "name": "Ahmet Candan",
    "list": [
        {
            "Date": 2024-10-29T19:23:43.511Z,
            "Amount": 12.34
        },
        {
            "Date": 2024-10-30T19:23:43.511Z,
            "Amount": 13.56
        }
    ]
}
```
Template
```html
Merhaba @name,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    $each(@i in @list){{
    <tr>
        <td>$DateFormat(@i.Date, 'dd/MM/yyyy')</td><td>$NumberFormat(@i.Amount, 'N')</td>
    </tr>}}
</table>

$if(@name == 'Ahmet'){{
    Name is @name
}}
$else{{
    Name is not Ahmet
}}

Send date: $Now('dd.MM.yyyy HH:mm')
```

Output
```html
Merhaba Ahmet Candan,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>

    <tr>
        <td>29/10/2024</td><td>12.34</td>
    </tr>
    <tr>
        <td>30/10/2024</td><td>13.56</td>
    </tr>
</table>

Name is Ahmet

Send date: 29.10.2024 19:23
```