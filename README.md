# HammingRecovery

Creative reimplementation of [Hamming Codes](https://en.wikipedia.org/wiki/Hamming_code) for .NET.

This is unusual implementation, it is more complex than traditional Hamming Codes, but it is intended for using in real applications. So performance is critical point for this library (Generally, for adding recovery info and checking correctness).

## Library state

**Alpha**, do not use it in production.

## What recovery can library do?

Don't expect to much from this library. It is not magical wand. It cannot restore damaged blocks of data. It can only repair **one damaged byte in some bytes**. If **two bytes** are damaged in one block, then recovery results will be unpredictable.

Usually, data lays in good state, because TCP uses checksums, HDD uses checksums, even server RAM uses checksums. But sometimes (usually by defective memory) some bits in some byte are inverted and data become invalid. One small byte in one big file. And all data are invalid (in reality, this is only one byte, but your data will have damaged structure, so, you won't be able to open it due broken checksum, invalid encryption, compression, or structure).

E.g. you have XML with data 

```<sometag>somedata</sometag>```

And data is slightly damaged:

```<somdtag>somedata</sometag>```

As result, you receive invalid open tag and invalid close tag. And invalid XML at all.

If you add recovery info to this data, you'll able to restore it automatically.

Some detailed info about size of recovery information:

Block Size | Recovery Info | Data Increase Pcnt
-----------|---------------|-------------------
4          | 3             | 75%
11         | 4             | 36%
26         | 5             | 19%
57         | 6             | 11%
120        | 7             | 6%
247        | 8             | 3%

**(Block sizes of greater size currently is not implemented due huge amount of work to calculate recovery info)**

## License

[MIT](https://github.com/force-net/HammingRecovery/blob/develop/LICENSE) license
