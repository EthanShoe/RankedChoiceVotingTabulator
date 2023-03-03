# RankedChoiceVotingTabulator
A C# .NET Core console application to take ranking question results from Microsoft Forms and tabulate them into ranked choice voting.

## Description
Microsoft Forms has a "ranking" question type available to put on a form. This question type uses a [point system](https://techcommunity.microsoft.com/t5/microsoft-forms/microsoft-forms-ranking-question-type-calculation-methodology/m-p/2057168#:~:text=i%20dont%20know%20the%20official%20name%2C%20but%20i%20like%20to%20think%20of%20it%20as%20points) to determine the "winner", but ranked choice voting can sometimes yield a different result.

The goal of RankedChoiceVotingTabulator is to convert the data gathered from the Microsoft Forms ranking question into a ranked choice voting result.

Downloading the data from a Microsoft Form (exported as an Excel file) puts the results from a ranking question into one column so that each cell has a string of the candidates in order from first to last delimited by a semicolon:

![Image of how column data is formatted](https://user-images.githubusercontent.com/28065361/222295710-f7569158-f268-4892-beed-4cfe10acf40b.png)

The RankedChoiceVotingTabulator will parse through said data calculate the winner based on ranked choice voting principles:

![Image of how data is formmated after tabulation](https://user-images.githubusercontent.com/28065361/222295708-8c1ed9b1-f0c2-4ebd-8338-7fa59623a667.png)

## How to Use

1. Download the latest release of RankedChoiceVotingTabulator
2. Download poll results from Microsoft Forms
    - Make sure that the file is not in protected view
    - Make sure that only result columns (column F and onward) are from ranked questions
2. Run the application with the Excel file as an argument
    - The easiest way to do this is to drag the Excel file over the application in File Explorer
3. Break any ties that occur during tabulation
    - There will be prompts for this should they happen

## Credit

The RankedChoiceVotingTabulator was created by [Ethan Shoe](https://github.com/EthanShoe).
