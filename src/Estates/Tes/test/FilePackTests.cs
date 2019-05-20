//#define LONGTEST
using Gamer.Estate.Tes.Records;
using Gamer.Format.Nif;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Xunit;
using Xunit.Abstractions;

namespace Gamer.Estate.Tes.Tests
{
    public class FilePackTests
    {
        public FilePackTests(ITestOutputHelper helper) => Core.Debug.LogFunc = x => helper.WriteLine(x.ToString());

        [Theory]
        [InlineData("game://Morrowind/Morrowind.bsa", "textures/Tx_BC_moss.dds")]
        public async Task LoadAssetPack(string path, string modelPath)
        {
            // given
            var assetPack = await new Uri(path).GetTesAssetPackAsync();
            // when
            var exist0 = assetPack.ContainsFile(modelPath);
            var data0 = await assetPack.LoadFileDataAsync(modelPath);
            // then
            Assert.True(exist0);
            Assert.NotNull(data0);
        }

        [Theory]
#if LONGTEST
        [InlineData("game://Morrowind/Morrowind.bsa")]
        [InlineData("game://Oblivion/Oblivion*")]
        [InlineData("game://SkyrimVR/Skyrim*")]
        [InlineData("game://Fallout4/Fallout4*")]
        [InlineData("game://Fallout4VR/Fallout4*")]
#else
        [InlineData("game://Morrowind/Morrowind.bsa")]
        [InlineData("game://Fallout4VR/Fallout4 - Materials.ba2")]
#endif
        public async Task LoadAssetPackAll(string path)
        {
            var assetPack = await new Uri(path).GetTesAssetPackAsync() as TesAssetPack;
            foreach (var pack in assetPack.Packs)
            {
                pack.TestContainsFile();
#if LONGTEST
                pack.TestLoadFileData(int.MaxValue);
#else
                pack.TestLoadFileData(100);
#endif
            }
        }

        [Theory]
#if LONGTEST
        [InlineData("game://Morrowind/Morrowind.esm")]
        [InlineData("game://Morrowind/Bloodmoon.esm")]
        [InlineData("game://Morrowind/Tribunal.esm")]
        [InlineData("game://Oblivion/Oblivion.esm")]
        [InlineData("game://SkyrimVR/Skyrim.esm")]
        [InlineData("game://Fallout4/Fallout4.esm")]
        [InlineData("game://Fallout4VR/Fallout4.esm")]
#else
        [InlineData("game://Morrowind/Morrowind.esm")]
#endif
        public void LoadDataPack(string path)
        {
            var dataPack = (TesDataPack)new Uri(path).GetTesDataPackAsync().Result;
            TestLoadCell(dataPack, new Vector3(0, 0, 0));
            TestAllCells(dataPack);

            ////TestLoadCell(new Vector3(((-2 << 5) + 1) * ConvertUtils.ExteriorCellSideLengthInMeters, 0, ((-1 << 5) + 1) * ConvertUtils.ExteriorCellSideLengthInMeters));
            ////TestLoadCell(new Vector3((-1 << 3) * ConvertUtils.ExteriorCellSideLengthInMeters, 0, (-1 << 3) * ConvertUtils.ExteriorCellSideLengthInMeters));
            //TestLoadCell(new Vector3(0 * ConvertUtils.ExteriorCellSideLengthInMeters, 0, 0 * ConvertUtils.ExteriorCellSideLengthInMeters));
            ////TestLoadCell(new Vector3((1 << 3) * ConvertUtils.ExteriorCellSideLengthInMeters, 0, (1 << 3) * ConvertUtils.ExteriorCellSideLengthInMeters));
            ////TestLoadCell(new Vector3((1 << 5) * ConvertUtils.ExteriorCellSideLengthInMeters, 0, (1 << 5) * ConvertUtils.ExteriorCellSideLengthInMeters));
            ////TestAllCells();

        }

        public static Vector3Int GetCellId(Vector3 point, int world) => new Vector3Int(Mathf.FloorToInt(point.x / ConvertUtils.ExteriorCellSideLengthInMeters), Mathf.FloorToInt(point.z / ConvertUtils.ExteriorCellSideLengthInMeters), world);

        static void TestLoadCell(TesDataPack data, Vector3 position)
        {
            var cellId = GetCellId(position, 60);
            var cell = data.FindCellRecord(cellId);
            var land = data.FindLANDRecord(cellId);
        }

        static void TestAllCells(TesDataPack data)
        {
            foreach (var record in data.Groups["CELL"].Records.Cast<CELLRecord>())
                Debug.Log(record.EDID.Value);
        }
    }
}
