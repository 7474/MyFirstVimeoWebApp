using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFirstVimeoWebApp.Models;
using VimeoDotNet;
using VimeoOpenApi.Api;
using VimeoOpenApi.Model;

namespace MyFirstVimeoWebApp.Controllers
{
    public class VideoController : Controller
    {
        private readonly ILogger<VideoController> logger;
        private readonly IVimeoConfig vimeoConfig;
        private readonly IVideosEssentialsApi videosEssentialsApi;
        private readonly IVideosUploadsApi videosUploadsApi;

        public VideoController(
            ILogger<VideoController> logger,
            IVimeoConfig vimeoConfig, IVideosEssentialsApi videosEssentialsApi, IVideosUploadsApi videosUploadsApi)
        {
            this.logger = logger;
            this.vimeoConfig = vimeoConfig;
            this.videosEssentialsApi = videosEssentialsApi;
            this.videosUploadsApi = videosUploadsApi;
        }
        // GET: VideoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: VideoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: VideoController/Create
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> UploadInfoAsync([FromQuery] uint size)
        {
            try
            {
                var res = await videosUploadsApi.UploadVideoAsyncWithHttpInfo(
                    vimeoConfig.AppUserId,
                    new InlineObject50(
                        upload: new MeVideosUpload(
                            approach: MeVideosUpload.ApproachEnum.Post,
                            size: size.ToString(),
                            // XXX さしあたって既定のVimeoのものを使う
                            redirectUrl: ""
                            ),
                        license: InlineObject50.LicenseEnum.By
                    )
                );

                // See. https://github.com/7474/VimeoOpenApi/issues/1
                var json = JsonDocument.Parse(res.RawContent);
                return Ok(json);

            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, ex.Message);

                // クオータ超えたとかの時は既存を取って置き換えにしてしのげるか？
                // -> アップロード用の情報は返却されない
                var res = await videosEssentialsApi.GetVideosAsyncWithHttpInfo(
                    vimeoConfig.AppUserId
                    );
                var json = JsonDocument.Parse(res.RawContent);
                return Ok(json.RootElement.EnumerateObject().First(x => x.Name == "data").Value[0]);
            }
        }

        // POST: VideoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: VideoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VideoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: VideoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
