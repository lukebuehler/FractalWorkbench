namespace FractalWorkbench.Core

type Class1() = 
    member this.X = "F#"

type Range = {x:(double*double); y:(double*double)}
type ViewPort = {width:int; height:int}

type IFractal = 
    abstract member DefaultRange : Range
    abstract member MaxTries : int
    abstract member GetFractal : Range -> ViewPort -> int[,]

