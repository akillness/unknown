// M16 runtime capture helper.
// Lists on-screen windows via CGWindowListCopyWindowInfo so the capture driver can
// target an explicit game window id for `screencapture -l` (never a desktop-wide grab).
// Output: windowNumber \t pid \t layer \t x \t y \t width \t height \t owner \t title
import CoreGraphics
import Foundation

let args = CommandLine.arguments
let ownerFilter = args.count > 1 ? args[1] : ""

guard let list = CGWindowListCopyWindowInfo(
    [.optionOnScreenOnly, .excludeDesktopElements], kCGNullWindowID) as? [[String: Any]] else {
    FileHandle.standardError.write("CGWindowListCopyWindowInfo failed\n".data(using: .utf8)!)
    exit(1)
}

for w in list {
    let owner = w[kCGWindowOwnerName as String] as? String ?? ""
    if !ownerFilter.isEmpty && !owner.contains(ownerFilter) { continue }
    let title = w[kCGWindowName as String] as? String ?? ""
    let num = w[kCGWindowNumber as String] as? Int ?? 0
    let pid = w[kCGWindowOwnerPID as String] as? Int ?? 0
    let layer = w[kCGWindowLayer as String] as? Int ?? 0
    let b = w[kCGWindowBounds as String] as? [String: Any] ?? [:]
    let x = b["X"] as? Double ?? 0
    let y = b["Y"] as? Double ?? 0
    let width = b["Width"] as? Double ?? 0
    let height = b["Height"] as? Double ?? 0
    print("\(num)\t\(pid)\t\(layer)\t\(Int(x))\t\(Int(y))\t\(Int(width))\t\(Int(height))\t\(owner)\t\(title)")
}
