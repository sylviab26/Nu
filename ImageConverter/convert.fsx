open System.Drawing
open System.Drawing.Imaging

#r "../packages/magick.net-q16-anycpu/7.14.0.1/lib/net40/Magick.NET-Q16-AnyCPU.dll"

open System
open System.IO

let directory = Environment.CurrentDirectory // root
let tilesDirectory = Path.Combine(directory, "Projects", "MyGame", "Assets", "Tiles")
let tilesDirectory2 = Path.Combine(directory, "Projects", "MyGame", "Assets", "Tiles32")
let directoryInfo = DirectoryInfo(tilesDirectory)
let directoryInfo2 = DirectoryInfo(tilesDirectory2)

if directoryInfo.Exists |> not then
  eprintfn "Директория не существует %s" directoryInfo.FullName
else
  directoryInfo.EnumerateFiles ("*.png")
  |> Seq.iter (fun fileInfo ->
    let destination = Path.Combine(directoryInfo2.FullName, fileInfo.Name)
    
    use img = Bitmap.FromFile(fileInfo.FullName)
    
    if img.PixelFormat = PixelFormat.Format32bppArgb |> not then
      let bm = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb)
      
      use g = Graphics.FromImage(bm)
      
      g.DrawImage(img, new Rectangle(0, 0, bm.Width, bm.Height))

      bm.Save(destination)
    else
      img.Save(destination)
    
    // 
    ()  
  )