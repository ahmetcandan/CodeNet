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
Merhaba Ahmet,
<table>
    <th>
        <td>Date</td><td>Amount</td>
    </th>
    
    <tr>
        <td>08-04-2025</td><td>12.46</td>
    </tr>
    <tr>
        <td>09-04-2025</td><td>15.90</td>
    </tr>
    <tr>
        <td>10-04-2025</td><td>13.00</td>
    </tr>
</table>


    Name is Ahmet


Send date: 08.04.2025 22:53
```