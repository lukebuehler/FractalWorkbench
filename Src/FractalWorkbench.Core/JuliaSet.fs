module JuliaSet

    open System
    open System.Numerics
    open FractalWorkbench.Core

    let maxTries = 30

    let next (z:Complex) (c:Complex) = z * z + c
    let willEscape (z:Complex) = z.Magnitude > 2.0

    let countIterations z c N =
        let rec countInner z c N i =
            if i >= N then i
            else
                if willEscape z then i
                else countInner (next z c) c N (i+1)
        countInner z c N 0

    let createEscapeArray range viewport c =
        let w = range.x
        let h = range.y
        let width = viewport.width
        let height = viewport.height
        let mutable data = Array2D.init width height (fun x y -> 0)

        for x in 0 .. width - 1 do
          for y in 0 .. height - 1 do 
            let x' = (float x / (float)width * (snd w - fst w)) + fst w
            let y' = (float y / (float)height * (snd h - fst h)) + fst h
            let it = countIterations (Complex(x', y')) c maxTries
            data.[x,y] <- it
        data

    type JuliSetFractal(c:Complex) =
        interface IFractal with
            member this.MaxTries = maxTries
            member this.DefaultRange = {x=(-2.0, 2.0); y=(-2.0, 2.0)}
            member this.GetFractal range viewport = createEscapeArray range viewport c
    

