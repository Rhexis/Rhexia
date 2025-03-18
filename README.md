# Rhexia (Rhex-I-A)

Rhexis is the interpreter for the language RIA, pronounced aria.

RIA stands for Rhexis Intelligence Artificielle, is a interpreted language written in C#.

Code Sample:
```
var name = "Rhexis";
var age = 30;
var daysOld = 10_950;

function greet(name)
{
    print("Hello, " + name);
}

greet(name);

print("Age is: " + age);
print("Last year your age was: " + age - 1);
print("Name is: " + name);

if (age == 30)
{
    print("You are 30 years old!");
}
else
{
    var ageInDays = age * 365;
    print(age + " is " + ageInDays + " days old");
}

if (!false) { print("true"); }

function addOne(num)
{
    return num + 1;
}

print(addOne(5));

var func = function()
{
    print("I am a closure");
}

func();

var arr = [1, 2, 3];
arr[0] = 100;
print(arr[0]);

var idx = 0;
while (idx < arr.Length)
{
    print(arr[idx]);
    idx++;
}

for (var i = 0; i < 10; i++)
{
    print("For:[" + i + "]");
}

struct object {
    name: "Rhexis",
    age: 30,
}
```