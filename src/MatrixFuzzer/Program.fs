// Learn more about F# at http://fsharp.org

open System

let chooseSize minRow maxRow minCol maxCol =
    let rand = System.Random ()
    let randomRow = rand.Next(minRow, maxRow+1)
    let randomCol = rand.Next(minCol, maxCol+1)
    (randomRow, randomCol)

let chooseDataFloat minData maxData =
    let rand = System.Random ()
    (float minData) + rand.NextDouble () * (float (maxData - minData))

let chooseDataInt minData maxData =
    let rand = System.Random ()
    rand.Next(minData, maxData+1)

let dataGeneratorFloat minData maxData () = chooseDataFloat minData maxData
let dataGeneratorInt minData maxData () = chooseDataInt minData maxData

type Matrix<'T> = {
    Row: int
    Col: int
    Data: List<List<'T>>
}

let createRandomMatrix row col (dataGen: unit -> int)  =
    {
        Row = row;
        Col = col;
        Data = List.init row (fun r -> List.init col (fun c -> dataGen ()))
    }

let formatMatrix matrix =
    "[" + ( matrix.Data
    |> List.map (fun row -> sprintf "%s" (row |> List.map (sprintf " %i ") |> List.reduce (+)))
    |> List.reduce (fun x y -> x + ";" + y)
    ) + "]"

[<EntryPoint>]
let main argv =
    let args = [
        argv.[0];
        argv.[1];
        argv.[2];
        argv.[3];
        argv.[4];
        argv.[5];
        argv.[6];
    ]

    let [ minRow; maxRow; minCol; maxCol; minData; maxData; fuzzingSize ] = args.[0..6] |> List.map int

    let generated =
        List.init fuzzingSize (fun i ->
            let (row, col ) = chooseSize minRow maxRow minCol maxCol
            createRandomMatrix row col (dataGeneratorInt minData maxData)
        )

    generated |> List.iter (formatMatrix >> printfn "A = %s; isequal(abs(rref(A) - my_rref(A)) < 1.0e-10, ones(size(A)))")

    0 // return an integer exit code
