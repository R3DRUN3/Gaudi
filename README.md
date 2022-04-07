# Gaudí
GitHub Audit Tool (PoC) 🔎 λ

## Abstract
Gaudí is a Proof of Concept for a GitHub audit service.

By taking advantage of *Github REST API* it is possible to implement customized audit procedures.

This console application 
is powered by [Octokit](https://github.com/octokit/octokit.net) dotnet library.

## Usage

Clone this repo: 

```
git clone https://github.com/R3DRUN3/Gaudi.git
```
Cd to the directory and run the program:
```
cd Gaudi && dotnet run gaudì
```
You can also build an executable, for example on Ubuntu 20.04 LTS:
```
dotnet build --configuration Release --runtime ubuntu.20.04-x64 
```

## Output sample
![Output](https://github.com/R3DRUN3/Gaudi/blob/main/images/gaudi_output.JPG)




