# RankedChoiceVotingTabulator

A C# .NET Desktop application to take ranking question results from Microsoft Forms and tabulate them into ranked choice voting.

## Description

Microsoft Forms has a "ranking" question type available to put on a form. This question type uses a [point system](https://techcommunity.microsoft.com/t5/microsoft-forms/microsoft-forms-ranking-question-type-calculation-methodology/m-p/2057168#:~:text=i%20dont%20know%20the%20official%20name%2C%20but%20i%20like%20to%20think%20of%20it%20as%20points) to determine the "winner", but ranked choice voting can sometimes yield a different result.

The goal of RankedChoiceVotingTabulator is to convert the data gathered from the Microsoft Forms ranking question into a ranked choice voting result.

Downloading the data from a Microsoft Form (exported as an Excel file) puts the results from a ranking question into one column so that each cell has a string of the candidates in order from first to last delimited by a semicolon:

![Image of how column data is formatted](https://github.com/EthanShoe/RankedChoiceVotingTabulator/assets/28065361/480fc898-3f8f-48f8-bda2-3e734ce7fd40)

The RankedChoiceVotingTabulator will parse through said data calculate the winner based on certain ranked choice voting principles [(how this application determines the winner)](https://github.com/EthanShoe/RankedChoiceVotingTabulator/blob/master/README.md#how-the-winner-is-determined):

![Image of how data is formatted after tabulation](https://github.com/EthanShoe/RankedChoiceVotingTabulator/assets/28065361/9e561fb4-865b-4e4b-b3ce-efc46b340950)

## How to Use

1. Download the latest release of RankedChoiceVotingTabulator
2. Download the poll results from Microsoft Forms (Excel file)
    - Make sure that the file is not in protected view
3. Run the application
4. Input the Excel file
5. Choose which columns from the file you want to tabulate
	- The application will automatically exclude columns that appear to not be from a ranked form question
6. Click Tabulate
7. Results are placed in new tabs on the file corresponding to each original ranked question
	- The Excel file will open automatically after tabulation is complete

## How the Winner Is Determined

In order for a candidate to win, they need to have more than 50% of the preferred votes. If nobody has more than 50% of the vote, then the candidate with the least amount of votes is removed from the running and anyone who voted for that candidate has their vote go towards their next preferred candidate.

This process is broken down into rounds. Each round, each candidate's preferred votes are tallied up (ex: If they were ranked first on any votes, those are counted. If they were ranked any place other than first, but they are the highest ranked candidate that has not been eliminated, those are also counted). If no candidate has more than 50% of the votes, the candidate with the lowest preferred votes is eliminated and a new round starts.

### Tie Breaking

In the event of a tie on any round, any candidates that are tied are eliminated together and the next round starts. This means that if there is a tie for the final winner, nobody wins.

Optionally, the user can choose to manually break ties. When this option is selected, the user will be prompted to eliminate a candidate any time two or more candidates have the lowest preferred vote count.

## Credit

The RankedChoiceVotingTabulator was created by [Ethan Shoe](https://github.com/EthanShoe).
